Imports System.Data
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data.SqlClient


Partial Class PersonasAtendidas
    Inherits Base
    '*****Variables para Usuario y contraseña encriptada

    Dim _errorLog As ErrorLog = New ErrorLog()

#Region "Eventos"
    Dim mensaje As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
     mensaje = Request.Params("Excel")

        If mensaje <> "" Then

            exportar()


        End If

        If Not Page.IsPostBack Then
            cargarcomboLugar()
            If ddlLugar.SelectedItem.Text = "Seleccionar.." Then
                ddlSector.Items.Add("Seleccionar..")
            Else
                cargarcomboSector()
                cargarcomboCod()
            End If





        Else

            If rdLug.SelectedValue = 0 Then

                txtDocumento.Enabled = True
                ddlSector.Visible = False
                lblSecDrop.Visible = False
                ddlSector.Enabled = True
                ddlLugar.Enabled = True
                ddlCod.Visible = False
                lblTNUCod.Visible = False

            ElseIf rdLug.SelectedValue = 1 Then

                txtDocumento.Enabled = True
                ddlSector.Visible = True
                lblSecDrop.Visible = True
                ddlSector.Enabled = True
                ddlLugar.Enabled = True
                ddlCod.Visible = False
                lblTNUCod.Visible = False

            ElseIf rdLug.SelectedValue = 2 Then

                txtDocumento.Enabled = True
                ddlSector.Visible = True
                lblSecDrop.Visible = True
                ddlSector.Enabled = True
                ddlLugar.Enabled = True
                ddlCod.Visible = True
                lblTNUCod.Visible = True

            End If
        End If

    End Sub
#End Region

#Region "Botones"
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Tomo_Valores_Iniciales(ddlSector.SelectedValue)
            BuscarCantidad()
            Me.btnDetalle.Text = "+Detalle"
            GvTurnos.DataSource = Nothing
            GvTurnos.DataBind()
        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try
    End Sub
    Protected Sub btnDetalle_Click(sender As Object, e As EventArgs) Handles btnDetalle.Click
        Try
            If Me.btnDetalle.Text = "+Detalle" Then
                Me.btnDetalle.Text = "-Detalle"
                Tomo_Valores_Iniciales(ddlSector.SelectedValue)
                BuscarNumeros()
                BuscarCantidad()
            Else
                Me.btnDetalle.Text = "+Detalle"
                GvTurnos.DataSource = Nothing
                GvTurnos.DataBind()
            End If

        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try
    End Sub


    Protected Sub GvTurnos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvTurnos.PageIndexChanging
        Try
            Me.GvTurnos.PageIndex = e.NewPageIndex
            Tomo_Valores_Iniciales(ddlSector.SelectedValue)
            If ddlTramite.Text = "Personas en espera" Then
                BuscarCantidad()
            ElseIf ddlTramite.Text = "Personas atendidas" Then
                BuscarCantidad()
            End If
        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try
    End Sub



    Protected Sub ddlLugar_TextChanged(sender As Object, e As EventArgs) Handles ddlLugar.TextChanged
        If ddlLugar.SelectedItem.Text = "Seleccionar.." Then
            ddlSector.Items.Clear()
            ddlSector.Items.Add("Seleccionar..")
            ddlCod.Items.Clear()
            ddlCod.Items.Add("Seleccionar..")
        Else
            cargarcomboSector()
            cargarcomboCod()
        End If
    End Sub




    Protected Sub ddlTramite_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTramite.SelectedIndexChanged
        'GvTurnos.DataSource = Nothing
        'GvTurnos.DataBind()
        'If ddlTramite.Text = "Personas en espera" Then
        '    Me.ddlLugar.Enabled = True
        '    Me.ddlSector.Enabled = True
        '    Me.txtFechaDesde.Enabled = False
        '    Me.txtFechaHasta.Enabled = False
        'ElseIf ddlTramite.Text = "Personas atendidas" Then
        '    Me.ddlLugar.Enabled = True
        '    Me.ddlSector.Enabled = True
        '    Me.txtFechaDesde.Enabled = True
        '    Me.txtFechaHasta.Enabled = True
        'ElseIf ddlTramite.Text = "Seleccionar" Then
        '    Me.ddlLugar.Enabled = False
        '    Me.ddlSector.Enabled = False
        '    Me.txtFechaDesde.Enabled = False
        '    Me.txtFechaHasta.Enabled = False
        'End If
    End Sub




    'Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
    '    GvTurnos.DataSource = Nothing
    '    GvTurnos.DataBind()
    '    Me.ddlLugar.Enabled = False
    '    Me.ddlSector.Enabled = False
    '    Me.txtFechaDesde.Enabled = False
    '    Me.txtFechaHasta.Enabled = False
    '    ddlTramite.Text = "Seleccionar"
    'End Sub




    'Protected Sub chkTodosL_CheckedChanged(sender As Object, e As EventArgs) Handles chkTodosL.CheckedChanged
    '    If chkTodosL.Checked = True Then
    '        Me.ddlLugar.Enabled = False
    '        ddlSector.Enabled = False
    '        Me.chkTodosS.Checked = True
    '        Me.chkTodosS.Enabled = False
    '    Else
    '        Me.ddlLugar.Enabled = True
    '        Me.chkTodosS.Enabled = True
    '    End If
    'End Sub

    'Protected Sub chkTodosS_CheckedChanged(sender As Object, e As EventArgs) Handles chkTodosS.CheckedChanged
    '    If chkTodosS.Checked = True Then
    '        ddlSector.Enabled = False
    '    Else
    '        ddlSector.Enabled = True
    '    End If
    'End Sub



  
    Protected Sub btnExcelSinDNILug_Click(sender As Object, e As EventArgs) Handles btnExcelSinDNILug.Click


        ExportarExcel(lblExcelSinDNILug.Text)
        Response.Redirect("PersonasAtendidas.aspx?Excel=1")
    End Sub
    Protected Sub btnExcelConDNILug_Click(sender As Object, e As ImageClickEventArgs) Handles btnExcelConDNILug.Click

        ExportarExcel(lblExcelConDNILug.Text)
        Response.Redirect("PersonasAtendidas.aspx?Excel=1")
    End Sub
    Protected Sub btnExcelSecSinDNI_Click(sender As Object, e As ImageClickEventArgs) Handles btnExcelSecSinDNI.Click

        ExportarExcel(lblExcelSecSinDNI.Text)
        Response.Redirect("PersonasAtendidas.aspx?Excel=1")
    End Sub
    
    Protected Sub btnExcelTotales_Click(sender As Object, e As ImageClickEventArgs) Handles btnExcelTotales.Click
        'Response.Redirect("PersonasAtendidas.aspx?Excel=" & ddlSector.SelectedValue & ddlLugar.SelectedValue & txtFechaDesde.Text & txtFechaHasta.Text & "ETOT")
        'ExportarExcel()
    End Sub

#End Region

#Region "Metodos"

    Public Sub cargarcomboLugar()
        Dim sql As String
        sql = "Select "
        sql = sql & " LUG_Descripcion, LUG_Numero"
        sql = sql & " from LugarCabecera "
        sql = sql & " where"
        sql = sql & " LUG_Sacaturnoseneldia = -1 "
        'If Session("Lugar").ToString <> "" Then
        '    sql = sql & " and  LUG_Numero= " & Session("Lugar").ToString & ""
        'End If
        CargoCombo(Me.ddlLugar, sql, ConexionGT)
        'If Session("Administrador").ToString Then
        '    Me.ddlLugar.Items.Add("Todos")
        'End If
    End Sub
    Public Sub cargarcomboSector()

        Dim sql As String
        sql = "Select "
        sql = sql & " SEC_Descripcion, SEC_Numero"
        sql = sql & " from Sectores "
        sql = sql & " where"
        sql = sql & " SEC_BajaLogica is null and  "
        sql = sql & " LUG_Numero = " & Me.ddlLugar.SelectedValue
        sql = sql & " Order by sec_descripcion "

        CargoCombo(Me.ddlSector, sql, ConexionGT)

    End Sub

    Public Sub cargarcomboCod()


        Dim sql As String

        sql = " SELECT DISTINCT TNU_Codigo as TCOD, SEC_Numero as SECN "
        sql = sql & " FROM Tipos_Numeros T "
        sql = sql & " WHERE T.SEC_Numero = " & Me.ddlSector.SelectedValue

        CargoCombo(Me.ddlCod, sql, ConexionGT)

    End Sub




    Protected Sub BuscarNumeros()
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

            sSql = "Select net.SEC_Numero as origen, LUG_Descripcion as lugar, SEC_Descripcion as sector, "
            sSql = sSql & " TNU_Codigo as letra, NEN_Numero as numero, NEN_Fecha as Fecha "
            sSql = sSql & " from NumerosEntregadosTesoreria net, sectores s, LugarCabecera lc  "
            sSql = sSql & " where "

            'If chkTodosL.Checked = False Then
            '    If chkTodosS.Checked = True Then
            '        sSql = sSql & " s.LUG_Numero = " & Me.ddlLugar.SelectedValue
            '    Else
            '        sSql = sSql & " net.SEC_Numero = " & Me.ddlSector.SelectedValue
            '    End If
            'Else
            '    sSql = sSql & " 1 = 1 "
            'End If

            sSql = sSql & " and NEN_LlamadoPor is null "
            sSql = sSql & " and NEN_HoraTerminado is null "
            sSql = sSql & " and day(NEN_Fecha) = day(getdate()) "
            sSql = sSql & " and month(NEN_Fecha) = month(getdate()) "
            sSql = sSql & " and year(NEN_Fecha) = year(getdate()) "
            sSql = sSql & " and s.SEC_Numero = net.SEC_Numero "
            sSql = sSql & " and s.LUG_Numero = lc.LUG_Numero "

            sSql = sSql & " Union "
            'Para derivados
            sSql = sSql & " Select net.SEC_Numero as origen, LUG_Descripcion as lugar, SEC_Descripcion as sector, "
            sSql = sSql & " TNU_Codigo as letra, NEN_Numero as numero, NEN_Fecha as Fecha "
            sSql = sSql & " from DerivacionesNET net, sectores s, LugarCabecera lc  "
            sSql = sSql & " where "

            'If chkTodosL.Checked = False Then
            '    If chkTodosS.Checked = True Then
            '        sSql = sSql & " s.LUG_Numero = " & Me.ddlLugar.SelectedValue
            '    Else
            '        sSql = sSql & " net.SEC_NumeroDerivado = " & Me.ddlSector.SelectedValue
            '    End If
            'Else
            '    sSql = sSql & " 1 = 1 "
            'End If

            sSql = sSql & " and NEN_Atendido = 'N' "
            sSql = sSql & " and NEN_CantVecesLlamado <= " & CantMaximaLlamadasDerivados
            sSql = sSql & " and day(NEN_Fecha) = day(getdate()) "
            sSql = sSql & " and month(NEN_Fecha) = month(getdate()) "
            sSql = sSql & " and year(NEN_Fecha) = year(getdate()) "
            sSql = sSql & " and getdate() > NEN_Fecha "
            sSql = sSql & " and DER_EsNumeroInternet = " & -1
            sSql = sSql & " and s.SEC_Numero = net.SEC_Numeroderivado "
            sSql = sSql & " and s.LUG_Numero = lc.LUG_Numero "

            sSql = sSql & " Union "
            'Para derivados de Internet
            sSql = sSql & " Select net.SEC_Numero as origen, LUG_Descripcion as lugar, SEC_Descripcion as sector, "
            sSql = sSql & " TNU_Codigo as letra, NEN_Numero as numero, NEN_Fecha as Fecha "
            sSql = sSql & " from DerivacionesNET net, sectores s, LugarCabecera lc  "
            sSql = sSql & " where "

            'If chkTodosL.Checked = False Then
            '    If chkTodosS.Checked = True Then
            '        sSql = sSql & " s.LUG_Numero = " & Me.ddlLugar.SelectedValue
            '    Else
            '        sSql = sSql & " net.SEC_NumeroDerivado = " & Me.ddlSector.SelectedValue
            '    End If
            'Else
            '    sSql = sSql & " 1 = 1 "
            'End If

            sSql = sSql & " and NEN_Atendido = 'N' "
            sSql = sSql & " and NEN_CantVecesLlamado <= " & CantMaximaLlamadasDerivados
            sSql = sSql & " and day(NEN_Fecha) = day(getdate()) "
            sSql = sSql & " and month(NEN_Fecha) = month(getdate()) "
            sSql = sSql & " and year(NEN_Fecha) = year(getdate()) "
            sSql = sSql & " and getdate() > NEN_Fecha "
            sSql = sSql & " and DER_EsNumeroInternet = " & 0
            sSql = sSql & " and s.SEC_Numero = net.SEC_Numeroderivado "
            sSql = sSql & " and s.LUG_Numero = lc.LUG_Numero "

            sql.CommandText = sSql

            daturnos = New SqlDataAdapter(sSql, con)
            '5-Se define un objeto dataset para volcar el resultado
            ds = New DataSet("PH")
            '6-Se descarga el resultado contenido en el objeto SQLAdapter al Dataset
            'con el metodo Fill
            daturnos.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                Me.GvTurnos.Visible = True

                Me.GvTurnos.DataSource = ds
                Me.GvTurnos.DataBind()

            Else
                lblMensajeError.Text = "No se encontraron datos"
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
                Me.GvTurnos.Visible = False
            End If
            con.Close()
        Catch er As Exception
            MsgBox(Err.Description)
        End Try
    End Sub

    Protected Sub BuscarCantidad()

        Dim con As System.Data.SqlClient.SqlConnection
        Dim daturnos As System.Data.SqlClient.SqlDataAdapter
        Dim sql As System.Data.SqlClient.SqlCommand
        Dim ds As DataSet
        Dim sSql As String


        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = ConexionGT()

        Try
            con.Open()
            If rdLug.SelectedValue = 0 Then
                sql = New System.Data.SqlClient.SqlCommand

                '    If (ddlLugar.Text = "Seleccionar.." And exceltipo = "") Or (exceltipo = "ESSD") Then

                If ddlLugar.Text = "Seleccionar.." Then

                    If txtDocumento.Text = "" Then
                        sSql = " Select count(*) as Cantidad, LUG_Descripcion as Lugar "
                        sSql = sSql & " from NumerosEntregadosTesoreria net, sectores s, LugarCabecera lc "
                        sSql = sSql & " where 1 = 1 "
                        sSql = sSql & " and NEN_HoraTerminado is not null "
                        sSql = sSql & " and s.SEC_Numero = net.SEC_Numero "
                        sSql = sSql & " and s.LUG_Numero = lc.LUG_Numero "

                        If txtFechaDesde.Text <> "" And txtFechaHasta.Text <> "" Then
                            sSql = sSql & " and net.NEN_Fecha between '" & CDate(txtFechaDesde.Text) & "' and '" & CDate(txtFechaHasta.Text) & "'"

                        End If

                        sSql = sSql & " group by LUG_Descripcion "
                        lblExcelSinDNILug.Text = sSql
                        sSql = sSql & " order by Cantidad ASC "

                        sql.CommandText = sSql

                        daturnos = New SqlDataAdapter(sSql, con)
                        ds = New DataSet("PH")
                        daturnos.Fill(ds)


                        If ds.Tables(0).Rows.Count > 0 Then
                            If mensaje = "" Then
                                Me.gvSinDNILug.Visible = True



                                Me.gvSinDNILug.DataSource = ds
                                Me.gvSinDNILug.DataBind()


                                Me.gvSinDNILug.Columns(1).Visible = False
                                Me.gvSinDNILug.Columns(0).Visible = False
                                Me.gvConDNILug.Visible = False
                                'Me.btnDetalle.Visible = True



                                ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalle", "gvSecSinDNIAbrir();", True)
                             

                            Else



                                lblMensajeError.Text = "No se encontraron datos"
                                ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)



                                Me.gvSinDNILug.Visible = False
                                'Me.btnDetalle.Visible = False
                            End If
                        Else

                        End If


                    Else

                        Me.gvSinDNILug.Visible = False
                        sSql = " Select count(*) as Cantidad, LUG_Descripcion as Lugar, TUR_Documento as DNI"
                        sSql = sSql & " from NumerosEntregadosTesoreria net, sectores s, LugarCabecera lc "
                        sSql = sSql & " where 1 = 1 "
                        sSql = sSql & " and net.TUR_Documento = " & Int(txtDocumento.Text)
                        sSql = sSql & " and NEN_HoraTerminado is not null "
                        sSql = sSql & " and s.SEC_Numero = net.SEC_Numero "
                        sSql = sSql & " and s.LUG_Numero = lc.LUG_Numero "

                        If txtFechaDesde.Text <> "" And txtFechaHasta.Text <> "" Then
                            sSql = sSql & " and net.NEN_Fecha between '" & CDate(txtFechaDesde.Text) & "' and '" & CDate(txtFechaHasta.Text) & "'"
                        End If

                        sSql = sSql & " group by LUG_Descripcion, TUR_Documento "
                        lblExcelConDNILug.Text = sSql
                        sSql = sSql & " order by Cantidad ASC "

                        sql.CommandText = sSql

                        daturnos = New SqlDataAdapter(sSql, con)
                        ds = New DataSet("PH")
                        daturnos.Fill(ds)
                        If ds.Tables(0).Rows.Count > 0 Then

                            Me.gvConDNILug.Visible = True
                            Me.gvConDNILug.DataSource = ds
                            Me.gvConDNILug.DataBind()



                            Me.gvSinDNILug.Columns(1).Visible = False
                            Me.gvSinDNILug.Columns(0).Visible = False
                            Me.gvSinDNILug.Visible = False





                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalle", "gvConDNILugAbrir();", True)

                            'Me.btnDetalle.Visible = True
                        Else
                            lblMensajeError.Text = "No se encontraron datos"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)

                            Me.gvConDNILug.Visible = False
                            'Me.btnDetalle.Visible = False
                        End If
                    End If

                Else



                    If txtDocumento.Text = "" Then
                        sSql = " Select count(*) as Cantidad, LUG_Descripcion as Lugar "
                        sSql = sSql & " from NumerosEntregadosTesoreria net, sectores s, LugarCabecera lc "
                        sSql = sSql & " where "

                        If rdLug.SelectedValue = 0 Then
                            sSql = sSql & " lc.LUG_Numero = " & ddlLugar.SelectedValue
                        End If

                        sSql = sSql & " and NEN_HoraTerminado is not null "
                        sSql = sSql & " and s.SEC_Numero = net.SEC_Numero "
                        sSql = sSql & " and s.LUG_Numero = lc.LUG_Numero "

                        If txtFechaDesde.Text <> "" And txtFechaHasta.Text <> "" Then
                            sSql = sSql & " and net.NEN_Fecha between '" & CDate(txtFechaDesde.Text) & "' and '" & CDate(txtFechaHasta.Text) & "'"

                        End If

                        sSql = sSql & " group by LUG_Descripcion "
                        lblExcelSinDNILug.Text = sSql
                        sSql = sSql & " order by Cantidad ASC "

                        sql.CommandText = sSql

                        daturnos = New SqlDataAdapter(sSql, con)
                        ds = New DataSet("PH")
                        daturnos.Fill(ds)
                        If ds.Tables(0).Rows.Count > 0 Then
                            If mensaje = "" Then
                                Me.gvSinDNILug.Visible = True
                                Me.gvSinDNILug.DataSource = ds
                                Me.gvSinDNILug.DataBind()



                                Me.gvSinDNILug.Columns(1).Visible = False
                                Me.gvSinDNILug.Columns(0).Visible = False
                                Me.gvConDNILug.Visible = False
                                'Me.btnDetalle.Visible = True


                                ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalle", "gvSinDNILugAbrir();", True)
                            Else
                                ExportaaExcel(ds)
                            End If
                        Else
                            lblMensajeError.Text = "No se encontraron datos"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)

                            Me.gvSinDNILug.Visible = False



                            'Me.btnDetalle.Visible = False
                        End If

                    Else

                        Me.gvSinDNILug.Visible = False
                        sSql = " Select count(*) as Cantidad, LUG_Descripcion as Lugar, TUR_Documento as DNI"
                        sSql = sSql & " from NumerosEntregadosTesoreria net, sectores s, LugarCabecera lc "

                        sSql = sSql & " where net.TUR_Documento = " & Int(txtDocumento.Text)

                        If rdLug.SelectedValue = 0 Then
                            sSql = sSql & " and lc.LUG_Numero = " & ddlLugar.SelectedValue
                        End If

                        sSql = sSql & " and NEN_HoraTerminado is not null "
                        sSql = sSql & " and s.SEC_Numero = net.SEC_Numero "
                        sSql = sSql & " and s.LUG_Numero = lc.LUG_Numero "

                        If txtFechaDesde.Text <> "" And txtFechaHasta.Text <> "" Then
                            sSql = sSql & " and net.NEN_Fecha between '" & CDate(txtFechaDesde.Text) & "' and '" & CDate(txtFechaHasta.Text) & "'"
                        End If

                        sSql = sSql & " group by LUG_Descripcion, TUR_Documento "
                        lblExcelConDNILug.Text = sSql
                        sSql = sSql & " order by Cantidad ASC "

                        sql.CommandText = sSql

                        daturnos = New SqlDataAdapter(sSql, con)
                        ds = New DataSet("PH")
                        daturnos.Fill(ds)
                        If ds.Tables(0).Rows.Count > 0 Then

                            Me.gvConDNILug.Visible = True
                            Me.gvConDNILug.DataSource = ds
                            Me.gvConDNILug.DataBind()


                            Me.gvSinDNILug.Columns(1).Visible = False
                            Me.gvSinDNILug.Columns(0).Visible = False
                            Me.gvSinDNILug.Visible = False

                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalle", "gvConDNILugAbrir();", True)

                            'Me.btnDetalle.Visible = True
                        Else
                            lblMensajeError.Text = "No se encontraron datos"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)

                            Me.gvConDNILug.Visible = False
                            'Me.btnDetalle.Visible = False
                        End If
                    End If

                End If

            ElseIf rdLug.SelectedValue = 1 Then

                sql = New System.Data.SqlClient.SqlCommand
                If ddlLugar.Text = "Seleccionar.." Then
                    If txtDocumento.Text = "" Then
                        sSql = " Select count(*) as Cantidad, LUG_Descripcion as Lugar, SEC_Descripcion as Sector"
                        sSql = sSql & " from NumerosEntregadosTesoreria net, sectores s, LugarCabecera lc "
                        sSql = sSql & " where 1 = 1 "
                        sSql = sSql & " and NEN_HoraTerminado is not null "
                        sSql = sSql & " and s.SEC_Numero = net.SEC_Numero "
                        sSql = sSql & " and s.LUG_Numero = lc.LUG_Numero "

                        If txtFechaDesde.Text <> "" And txtFechaHasta.Text <> "" Then
                            sSql = sSql & " and net.NEN_Fecha between '" & CDate(txtFechaDesde.Text) & "' and '" & CDate(txtFechaHasta.Text) & "'"
                        End If

                        sSql = sSql & " group by LUG_Descripcion, SEC_Descripcion "
                        Me.gvSecSinDNI.DataSource = ds
                        sSql = sSql & " order by Lugar, Sector ASC "

                        sql.CommandText = sSql

                        daturnos = New SqlDataAdapter(sSql, con)
                        ds = New DataSet("PH")
                        daturnos.Fill(ds)
                        If ds.Tables(0).Rows.Count > 0 Then

                            Me.gvTotales.Visible = False
                            Me.gvSecSinDNI.Visible = True

                            Me.gvSecSinDNI.DataBind()
                            lblExcelSecSinDNI.Text = sSql

                            Me.gvSecSinDNI.Columns(1).Visible = False
                            Me.gvSecSinDNI.Columns(0).Visible = False
                            'Me.btnDetalle.Visible = True
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalle", "gvSecSinDNIAbrir();", True)

                        Else
                            lblMensajeError.Text = "No se encontraron datos"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)

                            Me.gvSecSinDNI.Visible = False
                            Me.gvTotales.Visible = False
                            'Me.btnDetalle.Visible = False
                        End If

                    Else

                        sSql = " Select count(*) as Cantidad, LUG_Descripcion as Lugar, SEC_Descripcion as Sector, TUR_Documento as DNI"
                        sSql = sSql & " from NumerosEntregadosTesoreria net, sectores s, LugarCabecera lc "
                        sSql = sSql & " where 1 = 1 "
                        sSql = sSql & " and net.TUR_Documento = " & Int(txtDocumento.Text)
                        sSql = sSql & " and NEN_HoraTerminado is not null "
                        sSql = sSql & " and s.SEC_Numero = net.SEC_Numero "
                        sSql = sSql & " and s.LUG_Numero = lc.LUG_Numero "

                        If txtFechaDesde.Text <> "" And txtFechaHasta.Text <> "" Then
                            sSql = sSql & " and net.NEN_Fecha between '" & CDate(txtFechaDesde.Text) & "' and '" & CDate(txtFechaHasta.Text) & "'"
                        End If

                        sSql = sSql & " group by LUG_Descripcion, SEC_Descripcion, TUR_Documento "
                        lblExcelSecSinDNI.Text = sSql
                        sSql = sSql & " order by Cantidad ASC "

                        sql.CommandText = sSql

                        daturnos = New SqlDataAdapter(sSql, con)
                        ds = New DataSet("PH")
                        daturnos.Fill(ds)
                        If ds.Tables(0).Rows.Count > 0 Then

                            Me.gvSecSinDNI.Visible = False
                            Me.gvTotales.Visible = True
                            Me.gvTotales.DataSource = ds
                            Me.gvTotales.DataBind()


                            Me.gvTotales.Columns(1).Visible = False
                            Me.gvTotales.Columns(0).Visible = False
                            'Me.btnDetalle.Visible = True

                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalle", "gvTotalesAbrir();", True)


                        Else
                            lblMensajeError.Text = "No se encontraron datos"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)

                            Me.gvSecSinDNI.Visible = False
                            Me.gvTotales.Visible = False
                            'Me.btnDetalle.Visible = False
                        End If

                    End If

                Else

                    If txtDocumento.Text = "" Then
                        sSql = " Select count(*) as Cantidad, LUG_Descripcion as Lugar, SEC_Descripcion as Sector"
                        sSql = sSql & " from NumerosEntregadosTesoreria net, sectores s, LugarCabecera lc "
                        sSql = sSql & " where "

                        If rdLug.SelectedValue = 1 Then
                            sSql = sSql & " s.SEC_Numero = " & ddlSector.SelectedValue
                        End If

                        sSql = sSql & " and NEN_HoraTerminado is not null "
                        sSql = sSql & " and s.SEC_Numero = net.SEC_Numero "
                        sSql = sSql & " and s.LUG_Numero = lc.LUG_Numero "

                        If txtFechaDesde.Text <> "" And txtFechaHasta.Text <> "" Then
                            sSql = sSql & " and net.NEN_Fecha between '" & CDate(txtFechaDesde.Text) & "' and '" & CDate(txtFechaHasta.Text) & "'"
                        End If

                        sSql = sSql & " group by LUG_Descripcion, SEC_Descripcion "
                        lblExcelSecSinDNI.Text = sSql
                        sSql = sSql & " order by Cantidad ASC "

                        sql.CommandText = sSql

                        daturnos = New SqlDataAdapter(sSql, con)
                        ds = New DataSet("PH")
                        daturnos.Fill(ds)
                        If ds.Tables(0).Rows.Count > 0 Then

                            Me.gvTotales.Visible = False
                            Me.gvSecSinDNI.Visible = True
                            Me.gvSecSinDNI.DataSource = ds
                            Me.gvSecSinDNI.DataBind()



                            Me.gvSecSinDNI.Columns(2).Visible = False
                            Me.gvSecSinDNI.Columns(1).Visible = False
                            'Me.btnDetalle.Visible = True


                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalle", "gvSecSinDNIAbrir();", True)

                        Else
                            lblMensajeError.Text = "No se encontraron datos"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)

                            Me.gvSecSinDNI.Visible = False
                            Me.gvTotales.Visible = False
                            'Me.btnDetalle.Visible = False



                        End If

                    Else

                        sSql = " Select count(*) as Cantidad, LUG_Descripcion as Lugar, SEC_Descripcion as Sector, TUR_Documento as DNI"
                        sSql = sSql & " from NumerosEntregadosTesoreria net, sectores s, LugarCabecera lc "

                        sSql = sSql & " where net.TUR_Documento = " & Int(txtDocumento.Text)

                        If rdLug.SelectedValue = 1 Then
                            sSql = sSql & " and s.SEC_Numero = " & ddlSector.SelectedValue
                        End If

                        sSql = sSql & " and NEN_HoraTerminado is not null "
                        sSql = sSql & " and s.SEC_Numero = net.SEC_Numero "
                        sSql = sSql & " and s.LUG_Numero = lc.LUG_Numero "

                        If txtFechaDesde.Text <> "" And txtFechaHasta.Text <> "" Then
                            sSql = sSql & " and net.NEN_Fecha between '" & CDate(txtFechaDesde.Text) & "' and '" & CDate(txtFechaHasta.Text) & "'"
                        End If

                        sSql = sSql & " group by LUG_Descripcion, SEC_Descripcion, TUR_Documento "
                        lblExcelSecSinDNI.Text = sSql
                        sSql = sSql & " order by Cantidad ASC "

                        sql.CommandText = sSql

                        daturnos = New SqlDataAdapter(sSql, con)
                        ds = New DataSet("PH")
                        daturnos.Fill(ds)
                        If ds.Tables(0).Rows.Count > 0 Then

                            Me.gvSecSinDNI.Visible = False
                            Me.gvTotales.Visible = True
                            Me.gvTotales.DataSource = ds
                            Me.gvTotales.DataBind()


                            Me.gvTotales.Columns(1).Visible = False
                            Me.gvTotales.Columns(0).Visible = False
                            'Me.btnDetalle.Visible = True

                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalle", "gvTotalesAbrir();", True)


                        Else
                            lblMensajeError.Text = "No se encontraron datos"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)

                            Me.gvSecSinDNI.Visible = False
                            Me.gvTotales.Visible = False
                            'Me.btnDetalle.Visible = False
                        End If

                    End If

                End If

            ElseIf rdLug.SelectedValue = 2 Then

                sql = New System.Data.SqlClient.SqlCommand

                If ddlLugar.Text = "Seleccionar.." Then
                    If txtDocumento.Text = "" Then
                        sSql = " Select DISTINCT count(*) as Cantidad, LUG_Descripcion as Lugar, SEC_Descripcion as Sector, TN.TNU_Codigo as Numero "
                        sSql = sSql & " from NumerosEntregadosTesoreria net, sectores s, LugarCabecera lc, Tipos_Numeros TN "
                        sSql = sSql & " where 1 = 1 "
                        sSql = sSql & " and NEN_HoraTerminado is not null "
                        sSql = sSql & " and s.SEC_Numero = net.SEC_Numero "
                        sSql = sSql & " and s.LUG_Numero = lc.LUG_Numero "

                        If txtFechaDesde.Text <> "" And txtFechaHasta.Text <> "" Then
                            sSql = sSql & " and net.NEN_Fecha between '" & CDate(txtFechaDesde.Text) & "' and '" & CDate(txtFechaHasta.Text) & "'"
                        End If

                        sSql = sSql & " group by LUG_Descripcion, SEC_Descripcion, TN.TNU_Codigo "

                        sSql = sSql & " order by Lugar, Sector ASC "

                        sql.CommandText = sSql

                        daturnos = New SqlDataAdapter(sSql, con)
                        ds = New DataSet("PH")
                        daturnos.Fill(ds)
                        If ds.Tables(0).Rows.Count > 0 Then
                            Me.gvTotales.Visible = False
                            Me.gvNumero.Visible = True
                            Me.gvNumero.DataSource = ds
                            Me.gvNumero.DataBind()


                            Me.gvNumero.Columns(1).Visible = False
                            Me.gvNumero.Columns(0).Visible = False
                            'Me.btnDetalle.Visible = True



                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalle", "gvNumeroAbrir();", True)

                        Else
                            lblMensajeError.Text = "No se encontraron datos"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
                            Me.gvTotales.Visible = False
                            'Me.btnDetalle.Visible = False
                        End If

                    Else

                        sSql = " Select DISTINCT count(*) as Cantidad, LUG_Descripcion as Lugar, SEC_Descripcion as Sector, TUR_Documento as DNI, TN.TNU_Codigo as Numero "
                        sSql = sSql & " from NumerosEntregadosTesoreria net, sectores s, LugarCabecera lc, Tipos_Numeros TN "
                        sSql = sSql & " where 1 = 1 "
                        sSql = sSql & " and net.TUR_Documento = " & Int(txtDocumento.Text)
                        sSql = sSql & " and NEN_HoraTerminado is not null "
                        sSql = sSql & " and s.SEC_Numero = net.SEC_Numero "
                        sSql = sSql & " and s.LUG_Numero = lc.LUG_Numero "

                        If txtFechaDesde.Text <> "" And txtFechaHasta.Text <> "" Then
                            sSql = sSql & " and net.NEN_Fecha between '" & CDate(txtFechaDesde.Text) & "' and '" & CDate(txtFechaHasta.Text) & "'"
                        End If

                        sSql = sSql & " group by LUG_Descripcion, SEC_Descripcion, TUR_Documento, TN.TNU_Codigo "
                        sSql = sSql & " order by Cantidad ASC "

                        sql.CommandText = sSql

                        daturnos = New SqlDataAdapter(sSql, con)
                        ds = New DataSet("PH")
                        daturnos.Fill(ds)
                        If ds.Tables(0).Rows.Count > 0 Then
                            Me.gvTotales.Visible = False
                            Me.gvNumero.Visible = True
                            Me.gvNumero.DataSource = ds
                            Me.gvNumero.DataBind()
                            Me.gvNumero.Columns(1).Visible = False
                            Me.gvNumero.Columns(0).Visible = False
                            'Me.btnDetalle.Visible = True


                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalle", "gvNumeroAbrir();", True)
                        Else
                            lblMensajeError.Text = "No se encontraron datos"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
                            Me.gvTotales.Visible = False
                            'Me.btnDetalle.Visible = False
                        End If

                    End If

                Else

                    If txtDocumento.Text = "" Then
                        sSql = " Select DISTINCT count(*) as Cantidad, LUG_Descripcion as Lugar, SEC_Descripcion as Sector, TN.TNU_Codigo as Numero "
                        sSql = sSql & " from NumerosEntregadosTesoreria net, sectores s, LugarCabecera lc, Tipos_Numeros TN "
                        sSql = sSql & " where "

                        If rdLug.SelectedValue = 2 Then
                            sSql = sSql & " s.SEC_Numero = " & ddlSector.SelectedValue
                            sSql = sSql & " and TN.TNU_Codigo = " & "'" & Me.ddlCod.SelectedItem.Text & "'"
                        End If

                        sSql = sSql & " and NEN_HoraTerminado is not null "
                        sSql = sSql & " and s.SEC_Numero = net.SEC_Numero "
                        sSql = sSql & " and s.LUG_Numero = lc.LUG_Numero "

                        If txtFechaDesde.Text <> "" And txtFechaHasta.Text <> "" Then
                            sSql = sSql & " and net.NEN_Fecha between '" & CDate(txtFechaDesde.Text) & "' and '" & CDate(txtFechaHasta.Text) & "'"
                        End If

                        sSql = sSql & " group by LUG_Descripcion, SEC_Descripcion, TN.TNU_Codigo "
                        sSql = sSql & " order by Cantidad ASC "

                        sql.CommandText = sSql

                        daturnos = New SqlDataAdapter(sSql, con)
                        ds = New DataSet("PH")
                        daturnos.Fill(ds)
                        If ds.Tables(0).Rows.Count > 0 Then
                            Me.gvTotales.Visible = False
                            Me.gvNumero.Visible = True
                            Me.gvNumero.DataSource = ds
                            Me.gvNumero.DataBind()
                            Me.gvNumero.Columns(2).Visible = False
                            Me.gvNumero.Columns(3).Visible = False
                            'Me.btnDetalle.Visible = True

                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalle", "gvNumeroAbrir();", True)

                        Else
                            lblMensajeError.Text = "No se encontraron datos"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
                            Me.gvTotales.Visible = False
                            'Me.btnDetalle.Visible = False
                        End If

                    Else

                        sSql = " Select DISTINCT count(*) as Cantidad, LUG_Descripcion as Lugar, SEC_Descripcion as Sector, TUR_Documento as DNI, TN.TNU_Codigo as Numero "
                        sSql = sSql & " from NumerosEntregadosTesoreria net, sectores s, LugarCabecera lc, Tipos_Numeros TN "

                        sSql = sSql & " where net.TUR_Documento = " & Int(txtDocumento.Text)

                        If rdLug.SelectedValue = 2 Then
                            sSql = sSql & " and s.SEC_Numero = " & ddlSector.SelectedValue
                            sSql = sSql & " and TN.TNU_Codigo = " & "'" & Me.ddlCod.SelectedItem.Text & "'"
                        End If

                        sSql = sSql & " and NEN_HoraTerminado is not null "
                        sSql = sSql & " and s.SEC_Numero = net.SEC_Numero "
                        sSql = sSql & " and s.LUG_Numero = lc.LUG_Numero "

                        If txtFechaDesde.Text <> "" And txtFechaHasta.Text <> "" Then
                            sSql = sSql & " and net.NEN_Fecha between '" & CDate(txtFechaDesde.Text) & "' and '" & CDate(txtFechaHasta.Text) & "'"
                        End If

                        sSql = sSql & " group by LUG_Descripcion, SEC_Descripcion, TUR_Documento, TN.TNU_Codigo "
                        sSql = sSql & " order by Cantidad ASC "

                        sql.CommandText = sSql

                        daturnos = New SqlDataAdapter(sSql, con)
                        ds = New DataSet("PH")
                        daturnos.Fill(ds)
                        If ds.Tables(0).Rows.Count > 0 Then
                            Me.gvTotales.Visible = False
                            Me.gvNumero.Visible = True
                            Me.gvNumero.DataSource = ds
                            Me.gvNumero.DataBind()
                            Me.gvNumero.Columns(1).Visible = False
                            Me.gvNumero.Columns(0).Visible = False
                            'Me.btnDetalle.Visible = True


                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalle", "gvNumeroAbrir();", True)
                        Else
                            lblMensajeError.Text = "No se encontraron datos"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
                            Me.gvTotales.Visible = False
                            'Me.btnDetalle.Visible = False
                        End If

                    End If

                End If


                'ElseIf rdLug.SelectedValue = 2 Then

                '    sql = New System.Data.SqlClient.SqlCommand

                '    If txtDocumento.Text = "" Then

                '        sSql = " Select count(*) as Cantidad, LUG_Descripcion as Lugar, SEC_Descripcion as Sector"
                '        sSql = sSql & " from NumerosEntregadosTesoreria net, sectores s, LugarCabecera lc "
                '        sSql = sSql & " where 1 = 1 "
                '        sSql = sSql & " and NEN_HoraTerminado is not null "
                '        sSql = sSql & " and s.SEC_Numero = net.SEC_Numero "
                '        sSql = sSql & " and s.LUG_Numero = lc.LUG_Numero "

                '        If txtFechaDesde.Text <> "" And txtFechaHasta.Text <> "" Then
                '            sSql = sSql & " and net.NEN_Fecha between '" & CDate(txtFechaDesde.Text) & "' and '" & CDate(txtFechaHasta.Text) & "'"
                '        End If

                '        sSql = sSql & " group by LUG_Descripcion, SEC_Descripcion "
                '        sSql = sSql & " order by lugar ASC "

                '        sql.CommandText = sSql

                '        daturnos = New SqlDataAdapter(sSql, con)
                '        ds = New DataSet("PH")
                '        daturnos.Fill(ds)
                '        If ds.Tables(0).Rows.Count > 0 Then
                '            Me.lblBarra.Visible = True
                '            Me.lblSector.Visible = True
                '            Me.gvTotales.Visible = False
                '            Me.gvSecSinDNI.Visible = True
                '            Me.gvSecSinDNI.DataSource = ds
                '            Me.gvSecSinDNI.DataBind()
                '            Me.btnDetalle.Visible = True
                '        Else
                '            MessageBox("No se encontraron datos", Me)
                '            Me.lblBarra.Visible = False
                '            Me.lblSector.Visible = False
                '            Me.gvSecSinDNI.Visible = False
                '            Me.gvTotales.Visible = False
                '            Me.btnDetalle.Visible = False
                '        End If

            Else

            End If

            con.Close()
        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try
    End Sub



    Protected Sub Tomo_Valores_Iniciales(sector As String)
        'Dim snp As New ADODB.Recordset
        Dim con As System.Data.SqlClient.SqlConnection
        Dim sql As System.Data.SqlClient.SqlCommand
        Dim reader As System.Data.SqlClient.SqlDataReader
        Dim sSql As String
        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = ConexionGT()

        con.Open()
        sql = New System.Data.SqlClient.SqlCommand

        sSql = "Select  CantidadMaximaDeRellamadas "
        sSql = sSql & " from "
        sSql = sSql & " Sectores "

        If Me.ddlSector.SelectedValue <> "Seleccionar.." Then
            sSql = sSql & " where "
            sSql = sSql & " SEC_Numero = " & sector

        End If


        sql.CommandText = sSql

        sql.Connection = con
        reader = sql.ExecuteReader()
        reader.Read.ToString()

        CantMaximaLlamadasDerivados = reader.Item(0)

        reader.Close()
        con.Close()
    End Sub
    Public Sub ExportarExcel(sql2 As String)
        Dim con As System.Data.SqlClient.SqlConnection
        Dim daturnos As System.Data.SqlClient.SqlDataAdapter
        Dim sql As System.Data.SqlClient.SqlCommand
        Dim ds As DataSet
        Dim sSql As String
        sql = New System.Data.SqlClient.SqlCommand
        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = ConexionGT()

        sSql = "alter view EportarExcel as " & sql2

        sql.CommandText = sSql
        daturnos = New SqlDataAdapter(sSql, con)
        ds = New DataSet("PH")
        daturnos.Fill(ds)

    End Sub
    Public Sub exportar()
        Dim con As System.Data.SqlClient.SqlConnection
        Dim daturnos As System.Data.SqlClient.SqlDataAdapter
        Dim sql As System.Data.SqlClient.SqlCommand
        Dim ds As DataSet
        Dim sSql As String
        sql = New System.Data.SqlClient.SqlCommand
        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = ConexionGT()

        sSql = "select * from EportarExcel  "

        sql.CommandText = sSql
        daturnos = New SqlDataAdapter(sSql, con)
        ds = New DataSet("PH")
        daturnos.Fill(ds)
        ExportaaExcel(ds)
    End Sub

#End Region


    Public Sub sergio()

    End Sub


End Class
