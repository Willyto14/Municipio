Imports System.Data
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data.SqlClient

Partial Class Principal
    Inherits Base

    '*****Variables para Usuario y contraseña encriptada

    Dim _errorLog As ErrorLog = New ErrorLog()
#Region "Botones"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            _errorLog.Borrar()



            If Not Page.IsPostBack Then
                If (Session.Count < 1) Then
                    Response.Redirect("Default.aspx?Mensaje=" & 1)
                End If
                Dim Mensaje As String = (Request.Params("Mensaje"))
                If Mensaje <> "" Then
                    Dim mensajeError As New Mensajes()
                    lblmensaje.Text = mensajeError.Mensajes(Mensaje)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarMensaje();", True)
                End If
            End If


            Dim idll As New FuncionesBasicas.Funciones
            Dim sql As String

            If Not Page.IsPostBack Then
              
                    CargoComboStore(ddlTipoTramite, " exec dbo.LugaresdeTurnoNuevoPermisos " & Session("dni").ToString & "", "", ConexionGT)






                BuscoTipoNumero()

            End If
            If ddlTipoTramite.SelectedItem.Text <> "Central de Turnos Salud" Then
                If ddlTipoTramite.Items.Count > 0 Then

                    lblSeleccioneTramite.Visible = True
                    ddlTipoTramite.Visible = True
                    btnSiguiente.Visible = True


                    If TieneVariosTnu(ConexionGT, Me.ddlTipoTramite.SelectedItem.Value) > 1 Then
                        Me.ddlTipoNumero.Visible = True
                        lblTipoTramite.Visible = True
                        If Me.ddlTipoTramite.SelectedItem.Value = 65 Then
                            Me.panelzoo.Visible = True
                        Else
                            Me.panelzoo.Visible = False
                        End If
                    Else
                        Me.panelzoo.Visible = False
                        Me.ddlTipoNumero.Visible = False
                        lblTipoTramite.Visible = False
                    End If
                Else
                    Me.panelzoo.Visible = False
                    Me.ddlTipoNumero.Visible = False
                    lblTipoTramite.Visible = False
                    lblSeleccioneTramite.Visible = False
                    ddlTipoTramite.Visible = False
                    ddlTipoNumero.Visible = False
                    btnSiguiente.Visible = False
                End If

            End If
        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try
    End Sub

    Protected Sub btnSiguiente_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        Try
            If ddlTipoTramite.SelectedItem.Text = "Central de Turnos Salud" Then
                Response.Redirect("http://www.lomasdezamora.gov.ar/TurnosSalud.aspx")
            Else
                Session("Lugar") = Me.ddlTipoTramite.SelectedValue
                If ddlTipoNumero.Visible = True Then
                    Session("Numero") = Me.ddlTipoNumero.SelectedValue

                End If


                Dim dFechaInit As Date
                dFechaInit = InicioFecha(ConexionGT, Session("Lugar"), Session("Numero"))
                If dFechaInit <> "#12:00:00 AM#" Then
                    Response.Redirect("EntregaTurnos.aspx")
                Else
                    'MessageBoxOK("No hay turnos disponibles!", Me)
                    lblmensaje.Text = "No hay turnos disponibles!"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyErrorSave", "mostrarError();", True)

                End If
            End If
        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try
    End Sub


    Protected Sub ddlTipoTramite_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipoTramite.SelectedIndexChanged
        Try
            If ddlTipoTramite.SelectedItem.Text <> "Central de Turnos Salud" Then
                If TieneVariosTnu(ConexionGT, Me.ddlTipoTramite.SelectedItem.Value) > 1 Then
                    Me.ddlTipoNumero.Visible = True
                    lblTipoTramite.Visible = True
                    If Me.ddlTipoTramite.SelectedItem.Value = 65 Then
                        Me.panelzoo.Visible = True
                    Else
                        Me.panelzoo.Visible = False
                    End If
                Else
                    Me.ddlTipoNumero.Visible = False
                    lblTipoTramite.Visible = False
                End If


                BuscoTipoNumero()
            End If
        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try
    End Sub
    Protected Sub rdbDias_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rdbDias.SelectedIndexChanged
        Try
            BuscoTipoNumero()
        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try
    End Sub


    Protected Sub btnCerrarSesion_ServerClick(sender As Object, e As EventArgs) Handles btnCerrarSesion.ServerClick
        Try
            Session.Clear()
            Response.Redirect("Default.aspx")
        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try
    End Sub
#End Region

#Region "Metodos"
    Protected Sub BuscoTipoNumero()
        Dim sSql As System.Data.SqlClient.SqlCommand
        Dim con As System.Data.SqlClient.SqlConnection
        Dim ds As Data.DataSet
        Dim sqlAD As New SqlDataAdapter

        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = ConexionGT()
        Try
            con.Open()
            sSql = New System.Data.SqlClient.SqlCommand

            sSql.CommandText = " select TNU_Lugar, TNU_Codigo "
            sSql.CommandText = sSql.CommandText & " From Tipos_Numeros as t "
            sSql.CommandText = sSql.CommandText & " inner join sectores as s on s.SEC_Numero = t.SEC_Numero"
            sSql.CommandText = sSql.CommandText & " inner join LugarCabecera as l on l.Lug_Numero = s.Lug_Numero"
            sSql.CommandText = sSql.CommandText & " where s.Sec_Numero = " & ddlTipoTramite.SelectedValue
            sSql.CommandText = sSql.CommandText & " and t.TNU_EsViaInternet = -1 "
            If rdbDias.SelectedValue = "Lunes a Viernes" Then
                'Se carga Lunes a Viernes
                sSql.CommandText = sSql.CommandText & " and (TNU_Codigo <> 'C' and TNU_Codigo <> 'F') "
            Else
                'Se cargan los SABADOS
                sSql.CommandText = sSql.CommandText & " and (TNU_Codigo = 'C' or TNU_Codigo = 'F')"
            End If
            sqlAD = New SqlDataAdapter(sSql.CommandText, con)
            '5-Se define un objeto dataset para volcar el resultado
            ds = New DataSet("PH")
            '6-Se descarga el resultado contenido en el objeto SQLAdapter al Dataset
            'con el metodo Fill
            sqlAD.Fill(ds)
            con.Close()
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 1 Then
                    ddlTipoNumero.Visible = True
                    CargoCombo(ddlTipoNumero, sSql.CommandText, "", ConexionGT)
                Else
                    For Each dr As Data.DataRow In ds.Tables(0).Rows
                        ddlTipoNumero.Visible = False
                        Session("Numero") = dr.Item(1)
                    Next
                End If
            End If

        Catch er As Exception
            MessageBox(Err.Description.ToString)
        End Try
    End Sub

#End Region

  



End Class
