Imports System.Data
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data.SqlClient
Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Text
Imports System.Drawing
Imports System.IO
Imports SqlHelper
Partial Class PersonasenEspera
    Inherits Base
    Dim _errorLog As ErrorLog = New ErrorLog()
#Region "Eventos"

#Region "Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            cargarcomboLugar()
            cargarcomboSector()
            'ddlTramite.Items.Add("Seleccionar")
            ddlTramite.Items.Add("Personas en espera")
            'ddlTramite.Items.Add("Personas atendidas")


        End If

    End Sub

#End Region

#Region "Cambio de Pagina"
    Protected Sub GRTurnos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GRTurnos.PageIndexChanging
        GRTurnos.PageIndex = e.NewPageIndex

        BuscarNumeros()
    End Sub

#End Region

#End Region

#Region "Botones"

#Region "Buscar"
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Tomo_Valores_Iniciales()
            BuscarCantidad()
            GRTurnos.DataSource = Nothing
            GRTurnos.DataBind()

        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try
    End Sub
#End Region

#Region "Detalles"
    Protected Sub btnDetalle_Click(sender As Object, e As EventArgs) Handles btnDetalle.Click
        Try

            Tomo_Valores_Iniciales()
            BuscarNumeros()
            BuscarCantidad()
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalle", "mostrarDetalle();", True)



        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try
    End Sub

#End Region

#Region "ddlLugar"
    Protected Sub ddlLugar_TextChanged(sender As Object, e As EventArgs) Handles ddlLugar.TextChanged
        cargarcomboSector()
    End Sub

#End Region

#Region "ddlTramites"
    Protected Sub ddlTramite_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTramite.SelectedIndexChanged
        'GRTurnos.DataSource = Nothing
        'GRTurnos.DataBind()
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

#End Region

#Region "Cancelar"
    'Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
    '    GRTurnos.DataSource = Nothing
    '    GRTurnos.DataBind()
    '    Me.ddlLugar.Enabled = False
    '    Me.ddlSector.Enabled = False
    '    Me.txtFechaDesde.Enabled = False
    '    Me.txtFechaHasta.Enabled = False
    '    ddlTramite.Text = "Seleccionar"
    'End Sub

#End Region

#Region "Checks"
    Protected Sub chkTodosL_CheckedChanged(sender As Object, e As EventArgs) Handles chkTodosL.CheckedChanged
        If chkTodosL.Checked = True Then
            Me.ddlLugar.Enabled = False
            ddlSector.Enabled = False
            Me.chkTodosS.Checked = True
            Me.chkTodosS.Enabled = False
        Else
            Me.ddlLugar.Enabled = True
            Me.chkTodosS.Enabled = True
        End If
    End Sub

    Protected Sub chkTodosS_CheckedChanged(sender As Object, e As EventArgs) Handles chkTodosS.CheckedChanged
        If chkTodosS.Checked = True Then
            ddlSector.Enabled = False
        Else
            ddlSector.Enabled = True
        End If
    End Sub

#End Region

#End Region

#Region "Metodos"

#Region "Cargar combo"

    Public Sub cargarcomboLugar()

        Dim sql As String
        sql = "Select "
        sql = sql & " LUG_Descripcion, LUG_Numero"
        sql = sql & " from LugarCabecera "
        sql = sql & " where"
        sql = sql & " LUG_UsaLlamador = -1 "
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

#End Region

#Region "Cargar Turnos"
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

            If chkTodosL.Checked = False Then
                If chkTodosS.Checked = True Then
                    sSql = sSql & " s.LUG_Numero = " & Me.ddlLugar.SelectedValue
                Else
                    sSql = sSql & " net.SEC_Numero = " & Me.ddlSector.SelectedValue
                End If
            Else
                sSql = sSql & " 1 = 1 "
            End If

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

            If chkTodosL.Checked = False Then
                If chkTodosS.Checked = True Then
                    sSql = sSql & " s.LUG_Numero = " & Me.ddlLugar.SelectedValue
                Else
                    sSql = sSql & " net.SEC_NumeroDerivado = " & Me.ddlSector.SelectedValue
                End If
            Else
                sSql = sSql & " 1 = 1 "
            End If

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

            If chkTodosL.Checked = False Then
                If chkTodosS.Checked = True Then
                    sSql = sSql & " s.LUG_Numero = " & Me.ddlLugar.SelectedValue
                Else
                    sSql = sSql & " net.SEC_NumeroDerivado = " & Me.ddlSector.SelectedValue
                End If
            Else
                sSql = sSql & " 1 = 1 "
            End If

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
                Me.GRTurnos.Visible = True

                Me.GRTurnos.DataSource = ds
                Me.GRTurnos.DataBind()

            Else
                lblMensajeError.Text = "No se encontraron datos"
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
                Me.GRTurnos.Visible = False
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

            sql = New System.Data.SqlClient.SqlCommand

            sSql = " Select count(*) as cantidad, "
            sSql = sSql & " LUG_Descripcion as lugar, SEC_Descripcion as sector, 'NO' as derivados "
            sSql = sSql & " from NumerosEntregadosTesoreria net, sectores s, LugarCabecera lc  "
            sSql = sSql & " where "

            If chkTodosL.Checked = False Then
                If chkTodosS.Checked = True Then
                    sSql = sSql & " s.LUG_Numero = " & Me.ddlLugar.SelectedValue
                Else
                    sSql = sSql & " net.SEC_Numero = " & Me.ddlSector.SelectedValue
                End If
            Else
                sSql = sSql & " 1 = 1 "
            End If

            sSql = sSql & " and NEN_LlamadoPor is null "
            sSql = sSql & " and NEN_HoraTerminado is null "
            sSql = sSql & " and day(NEN_Fecha) = day(getdate()) "
            sSql = sSql & " and month(NEN_Fecha) = month(getdate()) "
            sSql = sSql & " and year(NEN_Fecha) = year(getdate()) "
            sSql = sSql & " and s.SEC_Numero = net.SEC_Numero "
            sSql = sSql & " and s.LUG_Numero = lc.LUG_Numero "
            sSql = sSql & " group by LUG_Descripcion, SEC_Descripcion "

            sSql = sSql & " Union "
            'Para derivados
            sSql = sSql & " Select count(*) as cantidad, "
            sSql = sSql & " LUG_Descripcion as lugar, SEC_Descripcion as sector, 'SI' as derivados "
            sSql = sSql & " from DerivacionesNET net, sectores s, LugarCabecera lc  "
            sSql = sSql & " where "

            If chkTodosL.Checked = False Then
                If chkTodosS.Checked = True Then
                    sSql = sSql & " s.LUG_Numero = " & Me.ddlLugar.SelectedValue
                Else
                    sSql = sSql & " net.SEC_NumeroDerivado = " & Me.ddlSector.SelectedValue
                End If
            Else
                sSql = sSql & " 1 = 1 "
            End If

            sSql = sSql & " and NEN_Atendido = 'N' "
            sSql = sSql & " and NEN_CantVecesLlamado <= " & CantMaximaLlamadasDerivados
            sSql = sSql & " and day(NEN_Fecha) = day(getdate()) "
            sSql = sSql & " and month(NEN_Fecha) = month(getdate()) "
            sSql = sSql & " and year(NEN_Fecha) = year(getdate()) "
            sSql = sSql & " and getdate() > NEN_Fecha "
            sSql = sSql & " and DER_EsNumeroInternet = " & -1
            sSql = sSql & " and s.SEC_Numero = net.SEC_Numeroderivado "
            sSql = sSql & " and s.LUG_Numero = lc.LUG_Numero "
            sSql = sSql & " group by LUG_Descripcion, SEC_Descripcion "

            sSql = sSql & " Union "
            'Para derivados de Internet
            sSql = sSql & " Select count(*) as cantidad, "
            sSql = sSql & " LUG_Descripcion as lugar, SEC_Descripcion as sector, 'SI (turno)' as derivados "
            sSql = sSql & " from DerivacionesNET net, sectores s, LugarCabecera lc  "
            sSql = sSql & " where "

            If chkTodosL.Checked = False Then
                If chkTodosS.Checked = True Then
                    sSql = sSql & " s.LUG_Numero = " & Me.ddlLugar.SelectedValue
                Else
                    sSql = sSql & " net.SEC_NumeroDerivado = " & Me.ddlSector.SelectedValue
                End If
            Else
                sSql = sSql & " 1 = 1 "
            End If

            sSql = sSql & " and NEN_Atendido = 'N' "
            sSql = sSql & " and NEN_CantVecesLlamado <= " & CantMaximaLlamadasDerivados
            sSql = sSql & " and day(NEN_Fecha) = day(getdate()) "
            sSql = sSql & " and month(NEN_Fecha) = month(getdate()) "
            sSql = sSql & " and year(NEN_Fecha) = year(getdate()) "
            sSql = sSql & " and getdate() > NEN_Fecha "
            sSql = sSql & " and DER_EsNumeroInternet = " & 0
            sSql = sSql & " and s.SEC_Numero = net.SEC_Numeroderivado "
            sSql = sSql & " and s.LUG_Numero = lc.LUG_Numero "
            sSql = sSql & " group by LUG_Descripcion, SEC_Descripcion "
            sSql = sSql & " order by LUG_Descripcion, SEC_Descripcion "

            sql.CommandText = sSql

            daturnos = New SqlDataAdapter(sSql, con)
            '5-Se define un objeto dataset para volcar el resultado
            ds = New DataSet("PH")
            '6-Se descarga el resultado contenido en el objeto SQLAdapter al Dataset
            'con el metodo Fill
            daturnos.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                Me.grTotales.Visible = True

                Me.grTotales.DataSource = ds
                Me.grTotales.DataBind()

                Me.btnDetalle.Visible = True

            Else
                lblMensajeError.Text = "No se encontraron datos"
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
                Me.grTotales.Visible = False
                Me.btnDetalle.Visible = False
            End If
            con.Close()
        Catch er As Exception
            MsgBox(Err.Description)
        End Try
    End Sub

#End Region

#Region "Rellamadas"
    Protected Sub Tomo_Valores_Iniciales()
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
        sSql = sSql & " where "
        sSql = sSql & " SEC_Numero = " & Me.ddlSector.SelectedValue

        sql.CommandText = sSql

        sql.Connection = con
        reader = sql.ExecuteReader()
        reader.Read.ToString()

        CantMaximaLlamadasDerivados = reader.Item(0)

        reader.Close()
        con.Close()
    End Sub

#End Region
    Public Sub willy()

    End Sub

#End Region






End Class
