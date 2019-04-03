Imports Encriptacion

Imports System.Data
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Partial Class Usuarios
    Inherits Base

    Dim Alta As Boolean
    Dim _validar As Validar = New Validar()
    Dim _errorLog As ErrorLog = New ErrorLog()

#Region "Botones"
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try

            If (Session.Count < 1) Then
                Response.Redirect("Default.aspx?Mensaje=" & 1)
            ElseIf Session("Administrador").ToString <> "True" Then
                Response.Redirect("Principal.aspx?Mensaje=" & 2)
            End If

            txtDniBUS.Attributes.Add("onkeyup", "setTimeout('__doPostBack(\'" + txtDniBUS.ClientID.Replace("_", "$") + "\',\'\')', 0);")
            txtNombreBUS.Attributes.Add("onkeyup", "setTimeout('__doPostBack(\'" + txtNombreBUS.ClientID.Replace("_", "$") + "\',\'\')', 0);")


            'Llama al metodo cargar grilla

            CargarGrilla()
        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try



    End Sub
    Protected Sub btnAltaModal_Click(sender As Object, e As EventArgs) Handles btnAltaModal.Click
        'LLama a Ventana Modal  Nuevo Usuario
        Try
            Blanqueo("Alta")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "altaModal", "altaAbrir();", True)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try

    End Sub
    Protected Sub btnAltaAceptarModal_Click(sender As Object, e As EventArgs) Handles btnAltaAceptarModal.Click
        'En la Ventana Modal Acepta los datos del Nuevo Usuario
        Try
            CrearUsuario()
        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try

    End Sub
    Protected Sub btnModificarAceptarModal_Click(sender As Object, e As EventArgs) Handles btnModificarAceptarModal.Click
        'En la Ventana Modal Acepta los datos de Modificacion de Usuario
        Try
            ModificarUsuario()
        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try

    End Sub
    Protected Sub btnEliminarAceptarModal_Click(sender As Object, e As EventArgs) Handles btnEliminarAceptarModal.Click
        'En la Ventana Modal Acepta los datos de Modificacion de Usuario
        Try
            EliminarUsuario()
        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try
    End Sub
    Protected Sub btnTurnoAceptarModal_Click(sender As Object, e As EventArgs) Handles btnTurnoAceptarModal.Click
        Try
            AgregarTurno()
        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)

        End Try


    End Sub

    Protected Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        'Acciones en la grilla Detalles , Modificar y Eliminar
        Try

            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            lblID.Text = Convert.ToInt32(dgvUsuarios.DataKeys(index).Value.ToString())

            If e.CommandName.Equals("detalleRecord") Then

                Dim ssql As String
                ssql = "   Select [CO_DNI]"
                ssql = ssql & "  ,[CO_Nombre]"
                ssql = ssql & " ,[CO_BajaLogica]"


                ssql = ssql & " , CASE  when [CO_Admin] = 'TRUE' then 'SI'"
                ssql = ssql & "  when [CO_Admin] <> 'TRUE' then 'NO'"
                ssql = ssql & "  End as Usr_EsAdministrador"

                ssql = ssql & " , CASE  when [CO_SobreTurno] = 'TRUE' then 'SI'"
                ssql = ssql & "  when [CO_SobreTurno] <> 'TRUE' then 'NO'"
                ssql = ssql & "  End as CO_SobreTurno"


                ssql = ssql & "   FROM [GestionTramites].[dbo].[O8OO]"
                ssql = ssql & " WHERE CO_DNI =  " & lblID.Text & ""



                CargoDetalles(ssql, Me.DetailsView1, ConexionLAB)

                'LLama a Ventana Modal Detalle de Usuario

                ScriptManager.RegisterStartupScript(Me, Me.GetType, "detalleModal", "mostrarDetalle();", True)

            End If

            If e.CommandName.Equals("editarRecord") Then
                Blanqueo("Editar")

                Dim gvrow As GridViewRow = dgvUsuarios.Rows(index)


                txtNombreUPD.Text = HttpUtility.HtmlDecode(gvrow.Cells(5).Text)



                If HttpUtility.HtmlDecode(gvrow.Cells(7).Text) = "SI" Then
                    chkAdministradorUPD.Checked = True
                Else
                    chkAdministradorUPD.Checked = False
                End If

                If HttpUtility.HtmlDecode(gvrow.Cells(8).Text) = "SI" Then
                    chkCChkSobreTurnoUPD.Checked = True
                Else
                    chkCChkSobreTurnoUPD.Checked = False
                End If

                'LLama a Ventana Modal editar de Usuario

                ScriptManager.RegisterStartupScript(Me, Me.GetType, "editarModal", "editarAbrir();", True)

            End If

            If e.CommandName.Equals("eliminarRecord") Then

                Dim gvrow As GridViewRow = dgvUsuarios.Rows(index)

                Dim Vacio As String = HttpUtility.HtmlDecode(gvrow.Cells(6).Text)




                If Vacio.Length < 2 Then
                    If Session("Dni") = HttpUtility.HtmlDecode(gvrow.Cells(4).Text) Then
                        lblMensajeErrorModal.Text = "No se puede dar de baja el usuario que esta usando"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
                    Else
                        lblMensajeBorrar.Text = "¿Esta Seguro que desea dar de Baja el usuario " & HttpUtility.HtmlDecode(gvrow.Cells(5).Text) & "?"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "eliminarModal", "eliminarAbrir();", True)
                    End If

                Else
                    lblMensajeBorrar.Text = "¿Esta Seguro que desea dar de Alta el usuario?"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "eliminarModal", "eliminarAbrir();", True)
                End If



            End If

            If e.CommandName.Equals("turnoRecord") Then

                CargarGrillaTurnos()


                ScriptManager.RegisterStartupScript(Me, Me.GetType, "turnoModal", "turnoAbrir();", True)

            End If
        Catch ex As Exception

            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)

        End Try
    End Sub
    Protected Sub dgvUsuarios_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgvUsuarios.PageIndexChanging
        dgvUsuarios.PageIndex = e.NewPageIndex
        CargarGrilla()
    End Sub

#End Region

    Protected Sub btnCerrarSesion_ServerClick(sender As Object, e As EventArgs) Handles btnCerrarSesion.ServerClick
        Try
            Session.Clear()
            Response.Redirect("Default.aspx")
        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try
    End Sub

#Region "Metodos"
    Public Sub CrearUsuario()
        'Metodo Acepta los datos del Nuevo Usuario

        Dim objEncriptar As New Encriptacion

        Dim sql As String
        sql = " INSERT INTO [GestionTramites].[dbo].[O8OO] "
        sql = sql & "  ([CO_DNI]"
        sql = sql & "  ,[CO_Nombre]"
        sql = sql & "  ,[CO_Pass]"
        sql = sql & "  ,[CO_Admin]"
        sql = sql & "  ,[CO_SobreTurno])"
        sql = sql & " VALUES( "
        sql = sql & "'" & UCase(txtDniINS.Text) & "'"
        sql = sql & ",'" & UCase(txtNombreINS.Text) & "'"
        sql = sql & ",'" & objEncriptar.EncriptarTexto(txtContraseñaINS.Text) & "'"
        If chkAdministradorINS.Checked = True Then
            sql = sql & " , 1 "
        Else
            sql = sql & " ,  0 "
        End If

        If chkCChkSobreTurnoINS.Checked = True Then
            sql = sql & " , 1 )"
        Else
            sql = sql & " ,  0 )"
        End If




        If ValidarDatos("Alta") Then
            If ComprobarUsuario(txtDniINS.Text) = False Then
                If ConsultaSQL(sql, ConexionLAB) = True Then
                    Me.CargarGrilla()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "altaModal", "altaCerrar();", True)
                End If
            Else
                txtDniINS.BorderColor = System.Drawing.Color.Red
                lblErrorInsertar.Text = "El Usuario ya existe"
                lblErrorInsertar.Visible = True
                txtDniINS.Focus()
            End If

        End If


    End Sub
    Public Sub ModificarUsuario()
        'Metodo Acepta los datos del Modificacion de Usuario

        Dim objEncriptar As New Encriptacion

        Dim sql As String


        Dim administrador As Boolean
        Dim sombreturno As Boolean

        sql = "    UPDATE [GestionTramites].[dbo].[O8OO]"
        sql = sql & "  SET  "
        sql = sql & "  [CO_Nombre] = '" & UCase(txtNombreUPD.Text) & "'"
        If chkAdministradorUPD.Checked = True Then
            sql = sql & ", CO_Admin = 1 "
            administrador = True
        Else
            sql = sql & ", CO_Admin =  0 "
            administrador = False
        End If
        If chkCChkSobreTurnoUPD.Checked = True Then
            sql = sql & ", CO_SobreTurno = 1 "
            sombreturno = True
        Else
            sql = sql & ", CO_SobreTurno =  0 "
            sombreturno = False
        End If
        sql = sql & " WHERE CO_DNI=" & lblID.Text

        If ValidarDatos("Editar") Then

            If ConsultaSQL(sql, ConexionLAB) = True Then
                Me.CargarGrilla()
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "editarModal", "editarCerrar();", True)
                If lblID.Text = Session("Dni").ToString Then
                    Session.Add("Administrador", administrador)
                    Session.Add("SobreTurno", sombreturno)
                End If

            End If
         
        End If



    End Sub
    Public Sub EliminarUsuario()
        'Metodo Acepta los datos de Eliminar de Usuario
        Dim message As String = String.Empty


        Dim mensaje As String

        Dim sql As String

        sql = "UPDATE [GestionTramites].[dbo].[O8OO]"
        sql = sql & "SET "
        If lblMensajeBorrar.Text <> "¿Esta Seguro que desea dar de Alta el usuario?" Then
            sql = sql & "CO_BajaLogica= getdate()"
            mensaje = "Se dio de baja el Usuario"
        Else
            sql = sql & "CO_BajaLogica=NULL"
            mensaje = "Se dio de Alta el Usuario"
        End If

        sql = sql & " WHERE  CO_DNI=" & lblID.Text & ""

        If ConsultaSQL(sql, ConexionLAB) = True Then
            Me.CargarGrilla()
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "eliminarModal", "eliminarCerrar();", True)
        End If

        CargarGrilla()




    End Sub
    Public Sub AgregarTurno()
        'Metodo utilizado para cargar Turno al usuario
        Dim consultarExistencia As Integer
        Dim consultaExitosa As Integer = 0

        Dim sql As String
        Dim ds As New DataSet




        For Each row As GridViewRow In gvMostrarTurno.Rows
            Dim check As CheckBox = TryCast(row.FindControl("chkPermisoHabilitado"), CheckBox)
            consultarExistencia = ConsultarSiTieneTurno(lblID.Text, row.Cells(1).Text)
            If check.Checked = True Then
                If consultarExistencia = 2 Then
                    'Inserto
                    sql = "   INSERT INTO O8OOTipoTurno"
                    sql = sql & " ([CO_DNI]"
                    sql = sql & " ,[TTI_ID])"
                    sql = sql & " VALUES"
                    sql = sql & "(" & lblID.Text
                    sql = sql & "," & row.Cells(1).Text & ")"

                    If ConsultaSQL(sql, ConexionGT) = True Then
                        consultaExitosa = 1
                    End If

                ElseIf consultarExistencia = 1 Then
                    'Update
                    sql = "      UPDATE O8OOTipoTurno "
                    sql = sql & "SET "
                    sql = sql & "  [OT_BajaLogica] = NULL "
                    sql = sql & " where CO_DNI =  " & lblID.Text
                    sql = sql & " and  TTI_ID =  " & row.Cells(1).Text

                    If ConsultaSQL(sql, ConexionGT) = True Then
                        consultaExitosa = 1
                    End If

                End If

            Else
                If consultarExistencia = 0 Then
                    'Update
                    sql = "      UPDATE O8OOTipoTurno "
                    sql = sql & "SET "
                    sql = sql & "  [OT_BajaLogica] = GetDate()"
                    sql = sql & " where CO_DNI =  " & lblID.Text
                    sql = sql & " and  TTI_ID =  " & row.Cells(1).Text

                    If ConsultaSQL(sql, ConexionGT) = True Then
                        consultaExitosa = 1
                    End If

                End If
            End If
        Next

        If consultaExitosa = 1 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "turnoModal", "turnoCerrar();", True)
        End If


    End Sub
    Public Function ComprobarUsuario(usuarios As String) As Boolean
        'Metodo para comprobar existencia del usuario

        Dim ds As New DataSet
        ComprobarUsuario = False
        Dim sql As String


        sql = " SELECT CO_Nombre From [GestionTramites].[dbo].[O8OO]"
        sql = sql & " where CO_DNI='" & UCase(usuarios) & "'"

        ds = EjecutarDataSet(sql, ConexionGT)

        If ds.Tables.Count > 0 Then
            If ds.Tables(0).Rows.Count > 0 Then
                ComprobarUsuario = True
            End If
        End If


        Return ComprobarUsuario
    End Function
    Public Sub CargarGrilla()
        'Metodo utilizado para cargar la Grilla



        Dim ssql As String = ""
        Dim ssql2 As String = ""





        ssql = "   Select [CO_DNI]"
        ssql = ssql & "  ,[CO_Nombre]"
        ssql = ssql & " ,[CO_BajaLogica]"

        ssql = ssql & " , CASE  when [CO_Admin] = 'TRUE' then 'SI'"
        ssql = ssql & "  when [CO_Admin] <> 'TRUE' then 'NO'"
        ssql = ssql & "  End as Usr_EsAdministrador"


        ssql = ssql & " , CASE  when [CO_SobreTurno] = 'TRUE' then 'SI'"
        ssql = ssql & "  when [CO_SobreTurno] <> 'TRUE' then 'NO'"
        ssql = ssql & "  End as CO_SobreTurno"

        ssql = ssql & "   FROM [GestionTramites].[dbo].[O8OO]"






        If txtDniBUS.Text <> "" Then
            ssql2 = " where CO_DNI LIKE  '%" & txtDniBUS.Text & "%'"
        End If
        If txtNombreBUS.Text <> "" Then
            If ssql2.ToString = "" Then
                ssql2 = ssql2 & "  where CO_Nombre LIKE  '%" & txtNombreBUS.Text & "%'"
            Else
                ssql2 = ssql2 & "  and CO_Nombre LIKE  '%" & txtNombreBUS.Text & "%'"
            End If

        End If


        If ssql2.ToString <> "" Then
            ssql = ssql & ssql2
        End If

        CargoGrilla(ssql, ConexionLAB, dgvUsuarios)

        dgvUsuarios.Visible = True



    End Sub
    Public Sub CargarGrillaTurnos()
        'Metodo utilizado para cargar la Grilla de la Turno



        Dim sql As String
        Dim ds As New DataSet
        sql = " Select [TTI_ID]"
        sql = sql & ", [TTI_Descripcion]"
        sql = sql & "  FROM [GestionTramites].[dbo].[TipoTurnoInternet]"
        sql = sql & "   where TTI_Habilitado = 1 "
        sql = sql & " order by TTI_Descripcion "


        CargoGrilla(sql, ConexionLAB, Me.gvMostrarTurno)

        sql = "  Select "

        sql = sql & " [TTI_ID]"
        sql = sql & "       FROM [GestionTramites].[dbo].[O8OOTipoTurno]"
        sql = sql & "       where CO_DNI =  " & lblID.Text
        sql = sql & " and OT_BajaLogica is null"


        ds = EjecutarDataSet(sql, ConexionLAB)


        If ds.Tables.Count > 0 Then
            If ds.Tables(0).Rows.Count > 0 Then


                For Each row As GridViewRow In gvMostrarTurno.Rows

                    For Each dr As Data.DataRow In ds.Tables(0).Rows
                        If row.Cells(1).Text = dr.Item(0) Then
                            Dim check As CheckBox = TryCast(row.FindControl("chkPermisoHabilitado"), CheckBox)
                            check.Checked = True

                        End If


                    Next

                Next
            End If
        End If


    End Sub

    Public Function ConsultarSiTieneTurno(Dni As Integer, Turno As Integer) As Integer
        'Metodo utilizado para Cosultar las Turno sanitarias que tiene el usuario

        '0 Tiene Asignado
        '1 Tiene Asignado Pero Dado de Baja
        '2 No Tiene Asignado


        Dim sql As String
        Dim ds As New DataSet


        sql = "  Select "
        sql = sql & "  OT_BajaLogica "
        sql = sql & " FROM [GestionTramites].[dbo].[O8OOTipoTurno]"
        sql = sql & " where CO_DNI =  " & Dni
        sql = sql & " and  TTI_ID =  " & Turno


        ds = EjecutarDataSet(sql, ConexionGT)


        If ds.Tables(0).Rows.Count > 0 Then
            For Each dr As Data.DataRow In ds.Tables(0).Rows
                If dr.Item(0) Is DBNull.Value Then
                    ConsultarSiTieneTurno = 0
                Else
                    ConsultarSiTieneTurno = 1
                End If
            Next
        Else
            ConsultarSiTieneTurno = 2
        End If


    End Function
    Public Function ValidarDatos(accion As String) As Boolean
        'Metodo utilizado para validar carga de datos
        ValidarDatos = True

        If accion = "Alta" Then
            ValidarDatos = _validar.ValidarTextboxCampoVacio(txtDniINS, lblErrorInsertar, ValidarDatos)
            ValidarDatos = _validar.ValidarTextboxCampoVacio(txtNombreINS, lblErrorInsertar, ValidarDatos)
            ValidarDatos = _validar.ValidarTextboxCampoVacio(txtContraseñaINS, lblErrorInsertar, ValidarDatos)
            ValidarDatos = _validar.ValidarTextboxCampoVacio(txtContraseñaRepetirINS, lblErrorInsertar, ValidarDatos)

            If ValidarDatos Then
                If txtContraseñaINS.Text <> txtContraseñaRepetirINS.Text Then
                    ValidarDatos = False
                    txtContraseñaINS.BorderColor = System.Drawing.Color.Red
                    lblErrorInsertar.Text = "Las contraseñas no coinciden"
                    lblErrorInsertar.Visible = True
                    txtContraseñaINS.Focus()
                Else
                    lblErrorInsertar.BorderColor = System.Drawing.Color.Empty
                End If
            End If


        End If

        If accion = "Editar" Then
            ValidarDatos = _validar.ValidarTextboxCampoVacio(txtNombreUPD, lblErrorModificar, ValidarDatos)
        End If



    End Function
    Public Sub Blanqueo(accion As String)
        'Metodo utilizado para para blaquear datos
        If accion = "Alta" Then
            Me.txtDniINS.Text = ""
            Me.txtNombreINS.Text = ""
            txtContraseñaINS.Text = ""
            txtContraseñaRepetirINS.Text = ""


            txtDniINS.BorderColor = System.Drawing.Color.Empty
            txtNombreINS.BorderColor = System.Drawing.Color.Empty
            txtContraseñaINS.BorderColor = System.Drawing.Color.Empty
            txtContraseñaRepetirINS.BorderColor = System.Drawing.Color.Empty
            lblErrorInsertar.Text = ""
            lblErrorInsertar.Visible = False
        End If

        If accion = "Editar" Then


            Me.txtNombreUPD.Text = ""



            txtNombreUPD.BorderColor = System.Drawing.Color.Empty
            lblErrorModificar.Text = ""
            lblErrorModificar.Visible = False
        Else


            Me.txtDniBUS.Text = ""
            Me.txtNombreBUS.Text = ""
            Me.lblID.Text = ""
            Me.dgvUsuarios.DataBind()
        End If
    End Sub

#End Region


  
End Class