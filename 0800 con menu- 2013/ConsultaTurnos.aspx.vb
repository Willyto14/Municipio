Imports System.Data
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine

Imports System.Data.SqlClient
Imports Microsoft.VisualBasic
Partial Class ConsultaTurnos
    Inherits Base
    Dim _errorLog As ErrorLog = New ErrorLog()
    Protected htmlPdf As String
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            Dim Mensaje As String = Request.Params("Excel")

            If Mensaje <> "" Then
                txtFechaD.Text =
                txtFechaH.Text = Mensaje.Substring(10, 10)
                Busca_Turnos(Mensaje.Substring(0, 10), Mensaje.Substring(10, 10), Mensaje.Substring(20, 1))
            End If


            If Not Page.IsPostBack Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyGuardarEdit", "PonerFechaDeHoy();", True)
                Me.lblFechaInicio.Text = FechaInicioLS(ConexionGT)
                Dim Fechainicio As Date = TopeFecha(ConexionGT, Session("Lugar"))

                Dim fechainicio2 = Format(Fechainicio, "dd/MM/yyyy")
                lblFecTopEN.Text = fechainicio2


                Dim FechaH As Date = Date.Today
                Dim FechaH2 = Format(FechaH, "dd/MM/yyyy")
                Dim FechaD As Date = Date.Today
                Dim FechaD2 = Format(FechaD, "dd/MM/yyyy")
                txtFechaD.Text = FechaD2
                txtFechaH.Text = FechaH
                txtFechaD.Focus()
                CargoComboStore(ddlTipoTramite, " exec dbo.LugaresdeTurnoNuevoPermisos " & Session("dni").ToString & "", "", ConexionGT)
                btnExportar.Visible = False
            End If
        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try
    End Sub

#End Region

#Region "Botones"


    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            buscarTurnos()

        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try
    End Sub
    Protected Sub GRTurnos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GRTurnos.RowCommand
        Try
            Dim sqlAD As New SqlDataAdapter
            Dim ds As New DataSet
            Dim Script As String
            Dim smen As String
            Dim dasql As New Data.SqlClient.SqlDataAdapter

            Session("index") = ""

            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = Me.GRTurnos.Rows(index)
            Session("index") = index

            If (e.CommandName = "ET") Then

                smen = "¿ Eliminar el Turno para : " & GRTurnos.Rows(index).Cells(1).Text.ToString
                smen = smen & " Fecha :" & GRTurnos.Rows(index).Cells(2).Text.ToString & " Hora : " & GRTurnos.Rows(index).Cells(3).Text.ToString
                smen = smen & " Documento: " & GRTurnos.Rows(index).Cells(4).Text.ToString & " Nombre :" & GRTurnos.Rows(index).Cells(6).Text.ToString & " ?"

                lblMensajeConfirmacion.Text = smen

                ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyEditEv", "confirmacionAbrir();", True)

            End If
            If (e.CommandName = "CF") Then
                If GRTurnos.Rows(index).Cells(11).Text.ToString = "No" Then
                    smen = "¿ Confirmar el Turno para : " & GRTurnos.Rows(index).Cells(1).Text.ToString
                    smen = smen & " Fecha :" & GRTurnos.Rows(index).Cells(2).Text.ToString & " Hora : " & GRTurnos.Rows(index).Cells(3).Text.ToString
                    smen = smen & " Documento: " & GRTurnos.Rows(index).Cells(4).Text.ToString & " Nombre :" & Replace(GRTurnos.Rows(index).Cells(6).Text.ToString, "'", "") & " ?"


                    lblMensajeConfirmacion.Text = smen

                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyEditEv", "confirmacionAbrir();", True)

                End If
            End If


        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try
    End Sub
    Protected Sub GRTurnos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GRTurnos.PageIndexChanging
        Try
            Me.GRTurnos.PageIndex = e.NewPageIndex
            buscarTurnos()
        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try
    End Sub
    Protected Sub btnVolver_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVolver.Click
        Try


            Response.Redirect("~/Turnos0800.aspx")
        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try
    End Sub
    Protected Sub btnAceptrarEliminar_Click(sender As Object, e As EventArgs) Handles btnAceptrarEliminar.Click
        Try
            Dim sqlAD As New SqlDataAdapter
            Dim ds As New DataSet
            Dim dred As System.Data.SqlClient.SqlDataReader
            Dim sSql As String = ""
            Dim dread As System.Data.SqlClient.SqlDataReader

            Try
                If GRTurnos.Rows(Session("index")).Cells(12).Text.ToString <> "Si" Then

                    dred = SqlHelper.ExecuteReader(ConexionGT, CommandType.StoredProcedure, "SP_BorraTurno0800Nuevo", _
                    New SqlParameter("@stnu", DBNull.Value), _
                    New SqlParameter("@tdni", DBNull.Value), _
                    New SqlParameter("@doc", DBNull.Value), _
                    New SqlParameter("@mail", DBNull.Value), _
                    New SqlParameter("@AtenV", DBNull.Value), _
                    New SqlParameter("@ApNom", DBNull.Value), _
                    New SqlParameter("@TelC", DBNull.Value), _
                    New SqlParameter("@conf", DBNull.Value), _
                    New SqlParameter("@ID", GRTurnos.Rows(Session("index")).Cells(8).Text.ToString))
                Else
                    GRTurnos.Columns(8).Visible = True
                    dred = SqlHelper.ExecuteReader(ConexionGT, CommandType.StoredProcedure, "SP_ReservaDeTurnoNUEVOWilSobreTurno", _
                   New SqlParameter("@ID", GRTurnos.Rows(Session("index")).Cells(8).Text.ToString), _
                   New SqlParameter("@Documento", GRTurnos.Rows(Session("index")).Cells(4).Text.ToString))


                End If
                dred.Read()

                If dred.Item(0) = 0 Then
                    'MessageBox("El turno no ha podido ser anulado!", Me)
                Else
                    Me.GRTurnos.Columns.Clear()
                    Me.GRTurnos.Visible = False
                    Dim FechaH As Date = Date.Today
                    Dim FechaH2 = Format(FechaH, "dd/MM/yyyy")
                    Dim FechaD As Date = Date.Today
                    Dim FechaD2 = Format(FechaD, "dd/MM/yyyy")
                    txtFechaD.Text = FechaD2
                    txtFechaH.Text = FechaH
                    txtFechaD.Focus()
                    Me.txtDni.Text = ""
                    txtFechaD.Focus()
                End If
                dred.Close()

            Catch er As Exception
                MsgBox(Err.Description)
            End Try


            If Session("Lugar") = 2 Then

                sSql = " UPDATE [GestionTramites].[dbo].[Turnos]"
                sSql = sSql & " SET [TUR_BajaLogica] = '01/01/2100'"
                sSql = sSql & " WHERE TUR_ID = '" & GRTurnos.Rows(Session("index")).Cells(8).Text.ToString & "'"

                Try

                    dread = SqlHelper.ExecuteReader(ConexionGT, Data.CommandType.Text, sSql)
                    dread.Read()
                Catch er As Exception
                    MsgBox(Err.Description)
                End Try
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyEditEv", "editarCerrar();", True)
            Cargo_Turnos()
        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try
    End Sub
    Protected Sub btnConfirmar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirmar.Click

        Try

            Dim sqlAD As New SqlDataAdapter
            Dim ds As New DataSet
            Dim dread As System.Data.SqlClient.SqlDataReader
            Dim sSql As String = ""


            sSql = " UPDATE [GestionTramites].[dbo].[Turnos]"
            sSql = sSql & " SET [TUR_Confirmado] = 1"
            sSql = sSql & " WHERE TUR_ID = '" & GRTurnos.Rows(Session("index")).Cells(8).Text.ToString & "'"

            Try


                dread = SqlHelper.ExecuteReader(ConexionGT, Data.CommandType.Text, sSql)
                dread.Read()
                txtFechaD.Focus()
                Cargo_Turnos()
            Catch er As Exception
                MsgBox(Err.Description)
            End Try
            dread.Close()
        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try
    End Sub
    Protected Sub GRTurnos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GRTurnos.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim KeyID As String = Me.GRTurnos.DataKeys(e.Row.RowIndex).Value.ToString()

                Dim imagen As System.Web.UI.WebControls.Image = DirectCast(e.Row.FindControl("imgestado"), System.Web.UI.WebControls.Image)

                If KeyID = "No" Then
                    imagen.ImageUrl = "~\img\Confirmar.ico"
                End If
                If KeyID = "Si" Then
                    imagen.ImageUrl = "~\img\Aceptado.bmp"
                End If
            End If
        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try
    End Sub
    Protected Sub cmdListar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportar.Click
        If txtFechaD.Text = "" And txtFechaH.Text = "" And Me.txtDni.Text = "" Then
            lblMensajeError.Text = "Complete la Fecha o el DNI"
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
            txtFechaD.Focus()

        Else
            If txtFechaD.Text <> "" And txtFechaH.Text <> "" Then
                If validarfecha() = True Then
                    listarPDF()
                Else
                    GRTurnos.Visible = False
                    txtFechaD.Focus()
                End If
            Else
                listarPDF()
            End If
        End If

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "imprimirAbrir();", True)
    End Sub
    Protected Sub btnExcel_Click(sender As Object, e As ImageClickEventArgs) Handles btnExcel.Click
        txtFechaDExpo.Text = txtFechaD.Text
        txtFechaHExpo.Text = txtFechaH.Text
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "excelAbrir();", True)
    End Sub
    Public Sub Listar_ResultadosSQL()

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
    Protected Sub btnPdf_Click(sender As Object, e As ImageClickEventArgs) Handles btnPdf.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarPDF();", True)
    End Sub
    Protected Sub btnAceptarEXCModal_Click(sender As Object, e As EventArgs) Handles btnAceptarEXCModal.Click
        Response.Redirect("ConsultaTurnos.aspx?Excel=" & txtFechaDExpo.Text & txtFechaHExpo.Text & rdbTurnos.SelectedValue)
    End Sub
    Protected Sub Busca_Turnos(fechaDesde As String, fechaHasta As String, opcion As Integer)
        Dim con As System.Data.SqlClient.SqlConnection
        Dim sql As System.Data.SqlClient.SqlCommand
        Dim daturnos As System.Data.SqlClient.SqlDataAdapter
        Dim ds As Data.DataSet
        Dim sSql As String




        sSql = "IF  EXISTS (SELECT * FROM sys.views WHERE object_id "
        sSql = sSql & " = OBJECT_ID(N'[dbo].[vTurnosSeleccion]'))"
        sSql = sSql & " DROP view [dbo].[vTurnosSeleccion]"

        SqlHelper.ExecuteReader(ConexionGT(), Data.CommandType.Text, sSql)

        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = ConexionGT()



        con.Open()
        sql = New System.Data.SqlClient.SqlCommand


        sSql = " Select "
        sSql = sSql & " lc.LUG_Descripcion as Origen, "
        sSql = sSql & " stn.STNU_Descripcion as Trámite, "
        sSql = sSql & " convert(varchar,T.TUR_Reservado, 103) as 'Fecha Reserva' , "
        sSql = sSql & " convert(varchar,T.TUR_Reservado, 108) as 'Hora Reserva', "
        sSql = sSql & " convert(varchar,T.TUR_FechaTurno, 103) as 'Fecha Turno'  , "
        sSql = sSql & " convert(varchar,T.TUR_FechaTurno, 108) as 'Hora Turno',   "
        sSql = sSql & " T.TUR_Documento as 'Documento',   "
        sSql = sSql & " T.TUR_Mail as 'Mail',    "
        sSql = sSql & " T.TUR_ApNomC as 'Ap.Nom',   "
        sSql = sSql & " T.TUR_TelContacto as 'Tel.Contacto',     "
        sSql = sSql & " T.TUR_ID as ID,   "
        sSql = sSql & " stn.STNU_Abreviatura as 'Tipo',   "
        sSql = sSql & " case  when T.ATVEN_DNI is null or T.ATVEN_DNI = '' then   "
        sSql = sSql & " 'Web'   "
        sSql = sSql & " else    "
        sSql = sSql & " T.ATVEN_DNI  "
        sSql = sSql & " end as 'Expedido por' ,  "
        sSql = sSql & " case  when  TUR_Confirmado"
        sSql = sSql & " = 'true' then    'si'    else     'no'   end as 'Confirmado'"
        sSql = sSql & " from "
        sSql = sSql & " Turnos T "
        sSql = sSql & " LEFT JOIN LugarCabecera lc  "
        sSql = sSql & " ON T.LUG_Numero = lc.LUG_Numero "
        sSql = sSql & " LEFT JOIN Sectores s "
        sSql = sSql & " ON T.SEC_Numero = S.SEC_Numero and "
        sSql = sSql & " T.LUG_Numero = S.LUG_Numero "
        sSql = sSql & " LEFT JOIN Tipos_Numeros tn "
        sSql = sSql & " ON T.SEC_Numero = tn.SEC_Numero and "
        sSql = sSql & " T.TNU_Codigo = tn.TNU_Codigo "
        sSql = sSql & " LEFT JOIN SubTipos_Numeros stn "
        sSql = sSql & " ON T.SEC_Numero = stn.SEC_Numero and "
        sSql = sSql & " T.TNU_Codigo = stn.TNU_Codigo and  "
        sSql = sSql & " T.STNU_Codigo = stn.STNU_Codigo "
        sSql = sSql & " where "


        Dim FechaH As Date = fechaHasta
        Dim FechaH2 = Format(FechaH, "yyyyMMdd")
        Dim FechaD As Date = fechaDesde
        Dim FechaD2 = Format(FechaD, "yyyyMMdd")

        If opcion <> 4 Then



            sSql = sSql & " TUR_FechaTurno <='" & FechaH2 & " 23:59:59' and  "
            sSql = sSql & " TUR_FechaTurno >='" & FechaD2 & " 00:00:00' and  "


        Else



            sSql = sSql & " TUR_Reservado <='" & FechaH2 & " 23:59:59' and  "
            sSql = sSql & " TUR_Reservado >='" & FechaD2 & " 00:00:00' and  "
            'sSql = sSql & " (ATVEN_DNI is not null and ATVEN_DNI <> '')  and "

        End If
        If opcion = 1 Then
            sSql = sSql & " ( T.TUR_Documento is null) "
        ElseIf opcion = 2 Then
            sSql = sSql & " ( T.TUR_Documento is not null) "

        Else
            sSql = sSql & " ( T.TUR_Documento is not null or T.TUR_Documento is null) "
        End If

        sSql = sSql & " Order by T.TUR_FechaTurno asc "


        sql.CommandText = sSql

        daturnos = New Data.SqlClient.SqlDataAdapter(sSql, con)
        '5-Se define un objeto dataset para volcar el resultado
        ds = New Data.DataSet("PH")
        '6-Se descarga el resultado contenido en el objeto SQLAdapter al Dataset
        'con el metodo Fill
        daturnos.Fill(ds)

        If ds.Tables(0).Rows.Count > 0 Then
            ExportaaExcel(ds)
        End If

        con.Close()



        Exit Sub




    End Sub
#End Region

#Region "Metodos"
    Public Sub buscarTurnos()
        If ValidarTurnos() = False Then


            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        Else
            Cargo_Turnos()

        End If

    End Sub
    Public Function ValidarTurnos() As Boolean
        ValidarTurnos = True
        If txtFechaD.Text = "" And txtFechaH.Text = "" And Me.txtDni.Text = "" Then
            lblMensajeError.Text = "Complete la Fecha o el DNI"
            txtFechaD.Focus()
            ValidarTurnos = False
        Else
            If txtFechaD.Text = "" And txtFechaH.Text <> "" Then
                lblMensajeError.Text = "Complete la Fecha Desde"
                txtFechaD.Focus()
                ValidarTurnos = False
            End If
            If txtFechaD.Text <> "" And txtFechaH.Text = "" Then
                lblMensajeError.Text = "Complete la Fecha Hasta"
                txtFechaH.Focus()
                ValidarTurnos = False
            End If
            If txtFechaD.Text <> "" And txtFechaH.Text <> "" Then
                If txtFechaD.Text > txtFechaH.Text Then
                    lblMensajeError.Text = "Fecha desde no puede ser mayor que fecha hasta"
                    txtFechaH.Focus()
                    ValidarTurnos = False
                End If
            End If
        End If
    End Function
    Protected Sub Cargo_Turnos()
        Dim con As System.Data.SqlClient.SqlConnection
        Dim daturnos As System.Data.SqlClient.SqlDataAdapter
        Dim sql As System.Data.SqlClient.SqlCommand
        Dim ds As DataSet
        Dim sSql As String


        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = ConexionGT()

        Try
            con.Open()

            sql = New System.Data.SqlClient.SqlCommand

            sSql = " Select "
            sSql = sSql & " lc.LUG_Descripcion as Origen, "
            sSql = sSql & " stn.STNU_Descripcion as Trámite, "
            sSql = sSql & " convert(varchar,T.TUR_FechaTurno, 103) as Fecha  ,   "
            sSql = sSql & " convert(varchar,T.TUR_FechaTurno, 108) as Hora,   "
            sSql = sSql & " T.TUR_Documento as 'Documento',   "
            sSql = sSql & " T.TUR_Mail as 'Mail',    "
            sSql = sSql & " T.TUR_ApNomC as 'Ap.Nom',   "
            sSql = sSql & " T.TUR_TelContacto as 'Tel.Contacto',     "
            sSql = sSql & " T.TUR_ID as ID,   "
            sSql = sSql & " stn.STNU_Abreviatura as 'Tipo',   "
            sSql = sSql & " case  when T.ATVEN_DNI is null or T.ATVEN_DNI = '' then   "
            sSql = sSql & " 'Web'   "
            sSql = sSql & " else    "
            sSql = sSql & " T.ATVEN_DNI  "
            sSql = sSql & " end as 'Expedido por' ,  "
            sSql = sSql & " case  when  TUR_Confirmado"
            sSql = sSql & " = 'true' then    'Si'    else     'No'   end as 'Confirmado',"
            sSql = sSql & " case  when  TUR_SobreTurno"
            sSql = sSql & " = 'true' then    'Si'    else     'No'   end as 'Sobre Turno'"
            sSql = sSql & " from "
            sSql = sSql & " Turnos T "
            sSql = sSql & " LEFT JOIN LugarCabecera lc  "
            sSql = sSql & " ON T.LUG_Numero = lc.LUG_Numero "
            sSql = sSql & " LEFT JOIN Sectores s "
            sSql = sSql & " ON T.SEC_Numero = S.SEC_Numero and "
            sSql = sSql & " T.LUG_Numero = S.LUG_Numero "
            sSql = sSql & " LEFT JOIN Tipos_Numeros tn "
            sSql = sSql & " ON T.SEC_Numero = tn.SEC_Numero and "
            sSql = sSql & " T.TNU_Codigo = tn.TNU_Codigo "
            sSql = sSql & " LEFT JOIN SubTipos_Numeros stn "
            sSql = sSql & " ON T.SEC_Numero = stn.SEC_Numero and "
            sSql = sSql & " T.TNU_Codigo = stn.TNU_Codigo and  "
            sSql = sSql & " T.STNU_Codigo = stn.STNU_Codigo "
            sSql = sSql & " where TUR_BajaLogica is NULL and"
            If txtFechaH.Text <> "" And txtFechaD.Text <> "" Then
                If Me.txtDni.Text <> "" Then
                    sSql = sSql & " TUR_Documento ='" & txtDni.Text & "' and  "
                End If


                Dim FechaH As Date = txtFechaH.Text
                Dim FechaH2 = Format(FechaH, "yyyyMMdd")
                Dim FechaD As Date = txtFechaD.Text
                Dim FechaD2 = Format(FechaD, "yyyyMMdd")



                sSql = sSql & " TUR_FechaTurno <='" & FechaH2 & " 23:59:59' and  "
                sSql = sSql & " TUR_FechaTurno >='" & FechaD2 & " 00:00:00' and  "



            ElseIf Me.txtDni.Text <> "" Then
                sSql = sSql & " TUR_Documento ='" & txtDni.Text & "' and  "
            End If
            If ddlTipoTramite.SelectedValue <> "Todos" Then
                sSql = sSql & " T.sec_Numero = " & ddlTipoTramite.SelectedValue
            End If

            'sSql = sSql & " and (T.TUR_Confirmado = 'True' or T.TUR_Confirmado = 'False'   and T.TUR_Documento is not null) "
            sSql = sSql & " Order by T.TUR_FechaTurno asc "


            sql.CommandText = sSql

            daturnos = New SqlDataAdapter(sSql, con)
            '5-Se define un objeto dataset para volcar el resultado
            ds = New DataSet("PH")
            '6-Se descarga el resultado contenido en el objeto SQLAdapter al Dataset
            'con el metodo Fill
            daturnos.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                Me.GRTurnos.Visible = True

                Me.GRTurnos.DataSource = ds
                Me.GRTurnos.DataBind()
                btnExportar.Visible = True
            Else

                Me.GRTurnos.Visible = False
                txtFechaD.Focus()
                btnExportar.Visible = False
            End If
        Catch er As Exception
            MsgBox(Err.Description)
        End Try
    End Sub
    Public Sub listarPDF()
        Dim sda As New SqlDataAdapter
        Dim reporte As New ReportDocument
        Dim con As New SqlConnection
        Dim Cuenta As Integer
        Dim sSql As String

        Try

            con = New SqlConnection(ConexionGT)
            con.Open()

            sSql = " ALTER procedure [dbo].[SP_TurnosSeleccion]  "
            sSql = sSql & " AS "
            sSql = sSql & " BEGIN "
            sSql = sSql & " SET NOCOUNT ON; "
            sSql = sSql & " Select     "
            sSql = sSql & " lc.LUG_Numero ,lc.LUG_Descripcion as Lugar, "
            sSql = sSql & " stn.STNU_Codigo ,stn.STNU_Descripcion as Trámite, "
            sSql = sSql & " T.TUR_FechaTurno as Fecha  ,   "
            sSql = sSql & " convert(varchar,T.TUR_FechaTurno, 108) as Hora,   "
            sSql = sSql & " T.TUR_Documento as 'Documento',   "
            sSql = sSql & " T.TUR_Mail as 'Mail',    "
            sSql = sSql & " T.TUR_ApNomC as 'Ap.Nom',   "
            sSql = sSql & " T.TUR_TelContacto as 'Tel.Contacto',     "
            sSql = sSql & " T.TUR_ID as ID,   "
            sSql = sSql & " stn.STNU_Abreviatura as 'Tipo',   "
            sSql = sSql & " case  when T.ATVEN_DNI  is null then   "
            sSql = sSql & " 'Web'   "
            sSql = sSql & " else    "
            sSql = sSql & " T.ATVEN_DNI  "
            sSql = sSql & " end as 'Expedido por'  , "
            sSql = sSql & " case  when  TUR_Confirmado"
            sSql = sSql & " = 'true' then    'si'    else     'no'   end as 'Confirmado'"
            sSql = sSql & " from "
            sSql = sSql & " Turnos T "
            sSql = sSql & " INNER JOIN LugarCabecera lc  "
            sSql = sSql & " ON T.LUG_Numero = lc.LUG_Numero "
            sSql = sSql & " INNER JOIN Sectores s "
            sSql = sSql & " ON T.SEC_Numero = S.SEC_Numero "
            'and "
            'sSql = sSql & " T.LUG_Numero = S.LUG_Numero "
            sSql = sSql & " INNER JOIN Tipos_Numeros tn "
            sSql = sSql & " ON T.SEC_Numero = tn.SEC_Numero and "
            sSql = sSql & " T.TNU_Codigo = tn.TNU_Codigo "
            sSql = sSql & " INNER JOIN SubTipos_Numeros stn "
            sSql = sSql & " ON T.SEC_Numero = stn.SEC_Numero and "
            sSql = sSql & " T.TNU_Codigo = stn.TNU_Codigo and  "
            sSql = sSql & " T.STNU_Codigo = stn.STNU_Codigo "
            sSql = sSql & " where "
            If txtFechaH.Text <> "" And txtFechaD.Text <> "" Then
                If Me.txtDni.Text <> "" Then
                    sSql = sSql & " TUR_Documento ='" & txtDni.Text & "' and  "
                End If
                Dim FechaH As Date = txtFechaH.Text
                Dim FechaH2 = Format(FechaH, "yyyyMMdd")
                Dim FechaD As Date = txtFechaD.Text
                Dim FechaD2 = Format(FechaD, "yyyyMMdd")


                sSql = sSql & " TUR_FechaTurno <='" & FechaH2 & " 23:59:59' and  "
                sSql = sSql & " TUR_FechaTurno >='" & FechaD2 & " 00:00:00' and  "


            ElseIf Me.txtDni.Text <> "" Then
                sSql = sSql & " TUR_Documento ='" & txtDni.Text & "' and  "
            End If

            sSql = sSql & " T.SEC_Numero = " & ddlTipoTramite.SelectedItem.Value & " and "
            sSql = sSql & " (T.TUR_Confirmado = 'True' or T.TUR_Confirmado = 'False'   and T.TUR_Documento is not null) "
            sSql = sSql & " Order by T.TUR_FechaTurno asc "
            sSql = sSql & " END "

            Dim command As New SqlCommand(sSql, con)
            Cuenta = command.ExecuteNonQuery()





            Dim ireport As New ReportDocument()
            Dim iConnectionInfo As ConnectionInfo = New ConnectionInfo()
            ' **********************************************************************************************
            ' configuro el acceso a la base de datos
            ' **********************************************************************************************
            iConnectionInfo.DatabaseName = "GestionTramites"
            iConnectionInfo.UserID = "cbconsulta"
            iConnectionInfo.Password = "cbconsulta"
            iConnectionInfo.ServerName = "srvprod"

            iConnectionInfo.Type = ConnectionInfoType.SQL






            If existencia().Rows.Count > 0 Then

                ireport.Load(Server.MapPath("~") & "\Reporte\Turnos.rpt")
                SetDBLogonForReport(iConnectionInfo, ireport)
                htmlPdf = Nombredepdf()
                ireport.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~") & "/" & htmlPdf)


            Else
                'MessageBox("No se encontraron datos", Me)
                Me.GRTurnos.Visible = False
                txtFechaD.Focus()
            End If


            con.Close()

        Catch er As Exception
            'MsgBox(Err.Description)
        End Try

    End Sub
    Private Sub SetDBLogonForReport(ByVal myConnectionInfo As ConnectionInfo, ByVal myReportDocument As ReportDocument)
        Dim myTables As Tables = myReportDocument.Database.Tables
        For Each myTable As CrystalDecisions.CrystalReports.Engine.Table In myTables
            Dim myTableLogonInfo As TableLogOnInfo = myTable.LogOnInfo
            myTableLogonInfo.ConnectionInfo = myConnectionInfo
            myTable.ApplyLogOnInfo(myTableLogonInfo)
        Next
    End Sub
    Public Function existencia() As DataTable
        Dim sSql As String = ""
        Dim dt As New DataTable()
        Dim con As New SqlConnection


        con = New SqlConnection(ConexionGT)
        con.Open()
        sSql = "SELECT * FROM Turnos WHERE"
        If txtFechaH.Text <> "" And txtFechaD.Text <> "" Then
            If Me.txtDni.Text <> "" Then
                sSql = sSql & " TUR_Documento ='" & txtDni.Text & "' and  "
            End If

            Dim FechaH As Date = txtFechaH.Text
            Dim FechaH2 = Format(FechaH, "yyyyMMdd")
            Dim FechaD As Date = txtFechaD.Text
            Dim FechaD2 = Format(FechaD, "yyyyMMdd")


            sSql = sSql & " TUR_FechaTurno <='" & FechaH2 & " 23:59:59' and  "
            sSql = sSql & " TUR_FechaTurno >='" & FechaD2 & " 00:00:00' and  "


        ElseIf Me.txtDni.Text <> "" Then
            sSql = sSql & " TUR_Documento ='" & txtDni.Text & "' and  "
        End If
        sSql = sSql & "  TUR_BajaLogica is null "


        Dim cmd As New SqlCommand(sSql, con)
        Dim adap As New SqlDataAdapter(cmd)
        adap.Fill(dt)

        Return dt
    End Function
    Public Sub TurnoTomadoxDocumentoInfo(ByVal hoy As String, ByVal hasta As String, ByVal doc As String)

        Dim con As System.Data.SqlClient.SqlConnection
        Dim reader As System.Data.SqlClient.SqlDataReader
        Dim sql As System.Data.SqlClient.SqlCommand


        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = ConexionGT()


        Try
            con.Open()


            sql = New System.Data.SqlClient.SqlCommand

            sql.CommandText = " Select TUR_Documento, TUR_FechaTurno as TieneTurno from Turnos "
            sql.CommandText = sql.CommandText & " where "
            sql.CommandText = sql.CommandText & " TUR_Documento = '" & doc & "' and "
            sql.CommandText = sql.CommandText & " TUR_FechaTurno >= '" & hoy & "' and "
            sql.CommandText = sql.CommandText & " TUR_FechaTurno <= '" & hasta & "'"

            sql.Connection = con
            reader = sql.ExecuteReader()
            reader.Read.ToString()


            If reader.Item(0) >= 1 Then
                Me.lblmens.Text = "El Nº de Doc :" & reader.Item(0).ToString & " ya posee un turno para la Fecha : " & reader.Item(1).ToString
            End If

            reader.Close()
            con.Close()
        Catch er As Exception
            MsgBox(Err.Description)
        End Try


    End Sub
    Public Function TurnoTomadoxDocumento(ByVal hoy As String, ByVal hasta As String, ByVal doc As String) As Boolean

        Dim con As System.Data.SqlClient.SqlConnection
        Dim reader As System.Data.SqlClient.SqlDataReader
        Dim sql As System.Data.SqlClient.SqlCommand


        TurnoTomadoxDocumento = False
        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = ConexionGT()


        Try
            con.Open()


            sql = New System.Data.SqlClient.SqlCommand

            sql.CommandText = " Select Count(*) as TieneTurno from Turnos "
            sql.CommandText = sql.CommandText & " where "
            sql.CommandText = sql.CommandText & " TUR_Documento = '" & doc & "' and "
            sql.CommandText = sql.CommandText & " TUR_FechaTurno >= '" & hoy & "' and "
            sql.CommandText = sql.CommandText & " TUR_FechaTurno <= '" & hasta & "'"

            sql.Connection = con
            reader = sql.ExecuteReader()
            reader.Read.ToString()


            If reader.Item(0) >= 1 Then
                TurnoTomadoxDocumento = True
            End If

            reader.Close()
            con.Close()
        Catch er As Exception
            MsgBox(Err.Description)
        End Try


    End Function
    Public Function DiaHabilitadoenTabla(ByVal Fecha As Date) As Boolean

        Dim con As System.Data.SqlClient.SqlConnection
        Dim reader As System.Data.SqlClient.SqlDataReader
        Dim sql As System.Data.SqlClient.SqlCommand


        DiaHabilitadoenTabla = False
        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = ConexionGT()


        Try
            con.Open()


            sql = New System.Data.SqlClient.SqlCommand
            sql.CommandText = " Select Count(*) as Existe from  Dias_y_Horarios "
            sql.CommandText = sql.CommandText & " where "
            sql.CommandText = sql.CommandText & " DYH_Habilitado = 1 and  "
            sql.CommandText = sql.CommandText & " DYH_Dia = " & Weekday(Fecha)

            sql.Connection = con
            reader = sql.ExecuteReader()
            reader.Read.ToString()


            If reader.Item(0) > 0 Then
                DiaHabilitadoenTabla = True
            End If


            reader.Close()
            con.Close()
        Catch er As Exception
            MsgBox(Err.Description)
        End Try


    End Function
    Sub DiaHabilitado1()
        Dim Cnn, rs

        'cnn = Server.CreateObject("ADODB.Connection")
        'cnn.open("PROVIDER=SQLOLEDB;DATA SOURCE=MUNICIPA-3;UID=cbconsulta;PWD=cbconsulta;DATABASE=GestionTramitesD")

        'Creamos el objeto de conexion ahora...
        Cnn = Server.CreateObject("ADODB.Connection")
        Cnn.Open("driver={SQL Server};server=MUNICIPA-3; database=GestionTramitesD; uid=cbconsulta;pwd=cbconsulta")

        ' Realizamos la consulta 
        rs = Cnn.Execute("SELECT * from Dias_y_Horarios ")
        ' Hacemos un bucle hasta que no 
        ' queden datos 
        While Not rs.EOF
            ' Escribimos los datos 
            Response.Write(rs.Fields("Nombre"))
            ' Pasamos al siguiente dato 
            rs.MoveNext()
        End While
        rs.Close()
        rs = Nothing
        Cnn.Close()
        Cnn = Nothing


    End Sub
    Function intervalos(ByVal txh As Integer) As Integer
        Dim interv As Integer

        'Valida el cociente de turnos por hora para determinar el intervalo
        If 60 / txh <= 10 Then
            interv = 10
        ElseIf 60 / txh > 10 And 60 / txh <= 15 Then
            interv = 15
        ElseIf 60 / txh > 15 And 60 / txh <= 20 Then
            interv = 20
        ElseIf 60 / txh > 20 And 60 / txh <= 30 Then
            interv = 30
        Else
            'interv = 1
            interv = 60
        End If
        intervalos = interv

    End Function
    Sub CargoComboTurnosHorario(ByVal Fecha As Date)

        Dim con As SqlConnection
        Dim sql As String
        Dim ds As DataSet
        Dim dahorarios As SqlDataAdapter


        Try


            'Breve resumen explicado de pasos a seguir
            '1-Se establece el objeto conexión 
            '2-Se abre la conexión 
            con = New SqlConnection(ConexionGT)
            con.Open()


            '3-Se escribe la consulta sql en variable tipo string
            sql = horadesdehasta(Weekday(Fecha))

            '4-El Objeto SqlAdapter se ejecuta en la conexión abierta
            'con la cadena sql de tipo string
            dahorarios = New SqlDataAdapter(sql, con)
            '5-Se define un objeto dataset para volcar el resultado
            ds = New DataSet("PH")
            '6-Se descarga el resultado contenido en el objeto SQLAdapter al Dataset
            'con el metodo Fill
            dahorarios.Fill(ds)
            'dahorarios.FillSchema(ds, SchemaType.Source)

            '7-Carga el combo con mierda 
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    For Each dr As DataRow In ds.Tables(0).Rows
                        'ddlturnosDisponibles.Items.Add(New ListItem(dr.Item(2)))
                    Next
                End If
            End If


            'reader.Close()
        Catch er As Exception
            MsgBox(Err.Description)
        End Try

    End Sub
    Function validarEmail(ByVal email)
        Dim partes, parte, i, c
        'rompo el email en dos partes, antes y después de la arroba
        partes = Split(email, "@")
        If UBound(partes) <> 1 Then
            'si el mayor indice del array es distinto de 1 es que no he obtenido las dos partes
            validarEmail = False
            Exit Function
        End If
        'para cada parte, compruebo varias cosas
        For Each parte In partes
            'Compruebo que tiene algún caracter
            If Len(parte) <= 0 Then
                validarEmail = False
                Exit Function
            End If
            'para cada caracter de la parte
            For i = 1 To Len(parte)
                'tomo el caracter actual
                c = LCase(Mid(parte, i, 1))
                'miro a ver si ese caracter es uno de los permitidos
                If InStr("._-abcdefghijklmnopqrstuvwxyz", c) <= 0 And Not IsNumeric(c) Then
                    validarEmail = False
                    Exit Function
                End If
            Next
            'si la parte actual acaba o empieza en punto la dirección no es válida
            If Left(parte, 1) = "." Or Right(parte, 1) = "." Then
                validarEmail = False
                Exit Function
            End If
        Next
        'si en la segunda parte del email no tenemos un punto es que va mal
        If InStr(partes(1), ".") <= 0 Then
            validarEmail = False
            Exit Function
        End If
        'calculo cuantos caracteres hay después del último punto de la segunda parte del mail
        i = Len(partes(1)) - InStrRev(partes(1), ".")
        'si el número de caracteres es distinto de 2 y 3
        If Not (i = 2 Or i = 3 Or i = 4) Then
            validarEmail = False
            Exit Function
        End If
        'si encuentro dos puntos seguidos tampoco va bien
        If InStr(email, "..") > 0 Then
            validarEmail = False
            Exit Function
        End If
        validarEmail = True
    End Function
    Function DatosValidos() As Boolean

        Dim desde, hasta As String
        DatosValidos = True
        Err.Clear()




        hasta = TopeFecha(ConexionGT, 10)
        desde = FechaInicio(ConexionGT)

    End Function
    Function ValidaTurnoTomado(ByVal hoy As Date, ByVal hasta As Date, ByVal doc As String) As Boolean
        Dim con As SqlConnection
        Dim sql As String
        Dim sqlAD As New SqlDataAdapter
        Dim ds As New DataSet

        ValidaTurnoTomado = False

        Try

            con = New SqlConnection(ConexionGT)
            con.Open()

            sql = " Select Count(*) as TieneTurno from Turnos "
            sql = sql & " where "
            sql = sql & " TUR_Documento = '" & doc & "' and "
            sql = sql & " TUR_FechaTurno >= '" & hoy & "' and "
            sql = sql & " TUR_FechaTurno >= '" & hasta & "'"

            sqlAD = New SqlDataAdapter(sql, con)
            '5-Se define un objeto dataset para volcar el resultado
            ds = New DataSet("PH")
            '6-Se descarga el resultado contenido en el objeto SQLAdapter al Dataset
            'con el metodo Fill
            sqlAD.Fill(ds)
            'dahorarios.FillSchema(ds, SchemaType.Source)
            'ANTES de grabar 
            If ds.Tables(0).Rows.Count > 0 Then
                'Tomo la hora de comienzo
                For Each dr As DataRow In ds.Tables(0).Rows
                    'horario inicio
                    'hi = dr.Item(0)
                    'horario finalización

                Next

            End If
        Catch er As Exception
            MsgBox(Err.Description)
        End Try


    End Function
    Function ValidaDoc0800(ByVal doc As String) As Boolean

        Dim con As SqlConnection
        Dim sql As String
        Dim sqlAD As New SqlDataAdapter
        Dim ds As New DataSet

        ValidaDoc0800 = False

        Try

            con = New SqlConnection(ConexionGT)
            con.Open()

            sql = " Select Count(*) as Existe from O8OO "
            sql = sql & " where "
            sql = sql & " CO_DNI = " & doc & ""

            sqlAD = New SqlDataAdapter(sql, con)
            '5-Se define un objeto dataset para volcar el resultado
            ds = New DataSet("PH")
            '6-Se descarga el resultado contenido en el objeto SQLAdapter al Dataset
            'con el metodo Fill
            sqlAD.Fill(ds)
            'dahorarios.FillSchema(ds, SchemaType.Source)
            'ANTES de grabar 

            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    For Each dr As DataRow In ds.Tables(0).Rows
                        If dr.Item(0) <> 0 Then
                            ValidaDoc0800 = True
                        End If
                    Next
                End If
            End If



        Catch er As Exception
            MsgBox(Err.Description)
        End Try


    End Function
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
    Public Function validarfecha() As Boolean
        validarfecha = True
        If txtFechaD.Text = "" Then

            lblMensajeError.Text = "Complete la Fecha"
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)

            txtFechaD.Focus()
            validarfecha = False
            Exit Function
        End If
        If txtFechaH.Text = "" Then
            lblMensajeError.Text = "Complete la Fecha"
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
            txtFechaH.Focus()
            validarfecha = False
            Exit Function
        End If
        If Not IsDate(txtFechaH.Text) Or Not IsDate(txtFechaD.Text) Then
            lblMensajeError.Text = "Complete las Fechas Correctamente"
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
            txtFechaD.Focus()
            validarfecha = False
            Exit Function
        End If


        If CDate(txtFechaH.Text) < CDate(txtFechaD.Text) Then
            lblMensajeError.Text = "Fecha Desde No puede ser mayor a fecha Hasta"
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
            txtFechaD.Focus()
            validarfecha = False
            Exit Function
        End If

    End Function
    Public Function Nombredepdf() As String
        Dim FechaBd As Date = traeerfechaGrabacion()
        Nombredepdf += "PDF/" & FechaBd.Day & "-" & FechaBd.Month & "-" & FechaBd.Year & "_" & FechaBd.Hour & "-" & FechaBd.Minute & "-" & FechaBd.Second & "-" & FechaBd.Millisecond & ".pdf"
    End Function
#End Region

End Class
