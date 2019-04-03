Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Text
Imports System.Drawing
Imports System.IO
Imports SqlHelper



Public Class Base
    Inherits System.Web.UI.Page
    Public CantMaximaLlamadasDerivados As Byte
#Region "Conexiones a Bases de Datos"
    Public Function TribunalDesarrollo() As String
        TribunalDesarrollo = "Data Source=srvprod.mlz2000;Initial Catalog=Tribunal_de_FaltasD; Integrated Security=False; User id=tflogin; password=sarli2012"
    End Function
    Public Function Tribunal() As String
        Tribunal = "Data Source=srvprod.mlz2000;Initial Catalog=Tribunal_de_Faltas; Integrated Security=False; User id=tflogin; password=sarli2012"
    End Function
    Public Function ParteDiario() As String
        ParteDiario = "Data Source=srvprod.mlz2000;Initial Catalog=ParteDiario; Integrated Security=False; User id=cbconsulta; password=cbconsulta"
    End Function

    Public Function ControlCombustible() As String
        ControlCombustible = "Data Source=srvprod.mlz2000;Initial Catalog=Control_Combustible; Integrated Security=False; User id=tflogin; password=sarli2012"
    End Function

    Public Function ConexionIMPTCE() As String
        ConexionIMPTCE = "Data Source=srvprod;Initial Catalog=Imptce; ;Integrated Security=False; User id=cbconsulta; password=cbconsulta; TimeOut =0 "
    End Function
    Public Function ConsultasGT() As String
        ConsultasGT = "Data Source=srvprod;Initial Catalog=GestionTramites; ;Integrated Security=False; User id=cbconsulta; password=cbconsulta; TimeOut =0 "
    End Function

    Public Function ConexionGT() As String
        ConexionGT = "Data Source=srvprod;Initial Catalog=GestionTramites; ;Integrated Security=False; User id=cbconsulta; password=cbconsulta; TimeOut =0 "
    End Function
    Public Function desarrolloMLZ() As String
        desarrolloMLZ = "Data Source=desarrollomlz;Initial Catalog=BD_munic1; ;Integrated Security=False; User id=CBTribunal; password=tribu4cb; TimeOut =0 "
    End Function
    Public Function srvprod2() As String
        srvprod2 = "Data Source=srvprod2;Initial Catalog=BD_Munic1; ;Integrated Security=False; User id=CBTribunal; password=tribu4cb; TimeOut =0 "
    End Function

    Public Function ConexionGTD() As String
        ConexionGTD = "Data Source=srvprod;Initial Catalog=GestionTramitesD; ;Integrated Security=False; User id=cbconsulta; password=cbconsulta; TimeOut =0 "
    End Function
    Public Function ConexionLAB() As String
        ConexionLAB = "Data Source=srvprod;Initial Catalog=Dispensario; ;Integrated Security=False; User id=cbconsulta; password=cbconsulta; TimeOut =0 "
    End Function

#End Region

#Region "Funciones y Metodos Comunes"

    Public Sub MessageBox(ByVal Mensaje As String)
        Dim lbl As New Label
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + Mensaje + "')</script>"
        Page.Controls.Add(lbl)
    End Sub
    Sub CargoCombo(ByVal cbo As DropDownList, ByVal sql As String, ByVal scondicion As String, ByVal sconexion As String)
        Dim dll As New FuncionesBasicas.Funciones
        Dim con As Data.SqlClient.SqlConnection
        Dim ds As Data.DataSet
        Dim dahorarios As Data.SqlClient.SqlDataAdapter

        cbo.Items.Clear()

        Try

            'Breve resumen explicado de pasos a seguir
            '1-Se establece el objeto conexión 
            '2-Se abre la conexión 
            con = New Data.SqlClient.SqlConnection(sconexion)
            con.Open()


            '3-Se escribe la consulta sql en variable tipo string
            'sql = dll.

            '4-El Objeto SqlAdapter se ejecuta en la conexión abierta
            'con la cadena sql de tipo string
            dahorarios = New Data.SqlClient.SqlDataAdapter(sql, con)
            '5-Se define un objeto dataset para volcar el resultado
            ds = New Data.DataSet("XXX")
            '6-Se descarga el resultado contenido en el objeto SQLAdapter al Dataset
            'con el metodo Fill
            dahorarios.Fill(ds)
            'dahorarios.FillSchema(ds, SchemaType.Source)

            '7-Carga el combo con mierda 
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    For Each dr As Data.DataRow In ds.Tables(0).Rows
                        cbo.Items.Add(New ListItem(dr.Item(0), dr.Item(1)))
                    Next
                End If
            End If
            con.Close()

            'reader.Close()
        Catch er As Exception
            MsgBox(Err.Description)
        End Try

    End Sub
    Sub CargoComboStore(ByVal cbo As DropDownList, ByVal store As String, ByVal scondicion As String, ByVal sconexion As String)
        Dim dll As New FuncionesBasicas.Funciones
        Dim ds As Data.DataSet

        cbo.Items.Clear()



        ds = SqlHelper.ExecuteDataset(ConexionGT, System.Data.CommandType.Text, store)

        If ds.Tables.Count > 0 Then
            If ds.Tables(0).Rows.Count > 0 Then
                For Each dr As Data.DataRow In ds.Tables(0).Rows
                    cbo.Items.Add(New ListItem(dr.Item(0), dr.Item(1)))


                Next
            End If
        End If


    End Sub
    Function CargoDsStore(ByVal cbo As DropDownList, ByVal store As String, ByVal scondicion As String, ByVal sconexion As String) As DataSet




        Dim index As Integer
        CargoDsStore = SqlHelper.ExecuteDataset(ConexionGT, System.Data.CommandType.Text, store)


        index = 0
        For Each item As Object In cbo.Items


            cbo.Items(index).Attributes.Add("style", "color: red")

            index = index + 1
        Next

        Dim inicio As Boolean = False

        If CargoDsStore.Tables(0).Rows.Count > 0 Then
            For Each row As DataRow In CargoDsStore.Tables(0).Rows
                index = 0
                For Each item As Object In cbo.Items

                    If row(0).ToString = item.ToString Then
                        cbo.Items(index).Attributes.Add("style", "color: black")
                        If inicio = False Then
                            cbo.SelectedItem.Attributes.Add("style", "color: black")
                            inicio = True
                        End If

                    Else
                        If inicio = False Then

                            cbo.SelectedItem.Attributes.Add("style", "color: red")
                            inicio = True
                        End If

                        'cbo.Items(index).Attributes.Add("style", "color: Red")
                        End If
                        index = index + 1
                Next


            Next



        End If


    End Function


   


    Public Function CargoGrilla(ByVal sql As String, ByVal conexion As String, ByVal grilla As GridView) As DataSet

        Dim con As System.Data.SqlClient.SqlConnection
        Dim dataAdapter As System.Data.SqlClient.SqlDataAdapter
        Dim ds As DataSet

        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = conexion


        con.Open()
        dataAdapter = New SqlDataAdapter(sql, con)
        '5-Se define un objeto dataset para volcar el resultado
        ds = New DataSet("PH")
        '6-Se descarga el resultado contenido en el objeto SQLAdapter al Dataset
        'con el metodo Fill
        dataAdapter.Fill(ds)


        grilla.DataSource = ds
        grilla.DataBind()



        con.Close()


        con.Close()
        Return ds

    End Function

    Public Function EjecutarDataSet(ByVal sql As String, ByVal conexion As String) As DataSet
        Dim con As System.Data.SqlClient.SqlConnection
        Dim daturnos As System.Data.SqlClient.SqlDataAdapter
        Dim ds As DataSet

        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = conexion


        con.Open()
        daturnos = New SqlDataAdapter(sql, con)
        '5-Se define un objeto dataset para volcar el resultado
        ds = New DataSet("PH")
        '6-Se descarga el resultado contenido en el objeto SQLAdapter al Dataset
        'con el metodo Fill
        daturnos.Fill(ds)

        con.Close()


        Return ds
    End Function
    'Funcion IUSQLID Retorna el id insertado en la base de datos
    Public Function IUSQLID(ByVal ssql As String) As Integer
        Dim ssqlID As String = "Select @@Identity"
        Dim ID As Integer
        Dim conn As New SqlClient.SqlConnection(ConexionGT)
        Dim cmd As New SqlClient.SqlCommand(ssql, conn)
        Try
            conn.Open()
            cmd.ExecuteNonQuery()
            cmd.CommandText = ssqlID
            ID = cmd.ExecuteScalar()
            conn.Close()
        Catch ex As Exception
            conn.Close()
        End Try

        Return ID
    End Function
 
    Public Function ConsultaSQL(ByVal sql As String, ByVal conexion As String) As Boolean

        Dim ds As New DataSet()
        Dim sda As New SqlClient.SqlDataAdapter()
        Dim cmd As New SqlClient.SqlCommand(sql)
        Dim con As New SqlConnection(conexion)

        cmd.CommandType = CommandType.Text
        cmd.Connection = con
        con.Open()

        sda.SelectCommand = cmd
        sda.Fill(ds)
        con.Close()
        ConsultaSQL = True

        con.Close()


    End Function
    Public Function ConsultaSQLDS(ByVal sql As String, conexion As String) As DataSet
        Dim con As New SqlConnection(conexion)
        Dim ds As New DataSet()
        Dim sda As New SqlClient.SqlDataAdapter()
        Dim cmd As New SqlClient.SqlCommand(sql)
        Try
            cmd.CommandType = CommandType.Text
            cmd.Connection = con
            con.Open()
            sda.SelectCommand = cmd
            sda.Fill(ds)
            con.Close()

        Catch ex As Exception
            con.Close()
        End Try
        Return ds
    End Function
    Public Sub ConsultaSQL(ByVal sql As String)
        Dim con As New SqlConnection(ConexionLAB)
        Dim ds As New DataSet()
        Dim sda As New SqlClient.SqlDataAdapter()
        Dim cmd As New SqlClient.SqlCommand(sql)
        Try
            cmd.CommandType = CommandType.Text
            cmd.Connection = con
            con.Open()
            sda.SelectCommand = cmd
            sda.Fill(ds)
            con.Close()

        Catch ex As Exception
            con.Close()
        End Try

    End Sub
    Public Sub CargoDetalles(ByVal sql As String, ByVal detalles As DetailsView, ByVal conexion As String)

        Dim ds As New DataSet()
        Dim sda As New SqlClient.SqlDataAdapter()
        Dim cmd As New SqlClient.SqlCommand(sql)
        Dim con As New SqlConnection(conexion)
        Try

            con.Open()

            cmd = New SqlCommand()
            cmd.CommandText = sql
            cmd.CommandType = CommandType.Text
            cmd.Connection = con
            sda = New SqlDataAdapter(cmd)
            ds = New DataSet()
            sda.Fill(ds)
            detalles.DataSource = ds
            detalles.DataBind()
            con.Close()
        Catch er As Exception
            con.Close()
        End Try
    End Sub
    Public Sub ExportaaExcel(ByVal ds As Data.DataSet)

        Dim strData As String = ""
        Dim bolFirstPass As Boolean = True
        ' Solo una vez para los encabezados
        strData = "<body><meta http-equiv='Content-Type' content='text/html;charset=utf-8'/>"
        strData &= "<table border=1>"
        strData &= "<tr>"
        For i As Integer = 0 To ds.Tables(0).Columns.Count - 1
            strData &= "<td>" & ds.Tables(0).Columns(i).ColumnName & "</td>"
        Next
        strData &= "</tr>"
        bolFirstPass = False
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("content-disposition", "attachment; filename=TurnosSeleccion.xls")
        Response.Write(strData)

        'Para el resto del Archivo
        For h As Integer = 0 To ds.Tables(0).Rows.Count - 1
            strData = "<tr>"
            For x As Integer = 0 To ds.Tables(0).Columns.Count - 1
                strData &= "<td>" & ds.Tables(0).Rows(h).Item(x).ToString & "</td>"
            Next x
            strData &= "</tr>"
            Response.Write(strData)
        Next h

        Response.Write("</table></body>")
        Response.End()

    End Sub
    Function traeerfechaGrabacion() As DateTime
        Dim base As New Base
        Dim Sql As String
        Dim dt As New DataTable()
        Dim con As New SqlConnection(base.ConsultasGT)
        Dim sda As New SqlDataAdapter()


        Sql = "select getdate();"




        Try
            Dim cmd As New SqlCommand(Sql)
            cmd.CommandType = CommandType.Text
            cmd.Connection = con

            con.Open()
            sda.SelectCommand = cmd
            sda.Fill(dt)
            con.Close()


            If dt.Rows.Count > 0 Then


                For Each row As DataRow In dt.Rows

                    Return row(0).ToString



                Next
            Else
                MessageBox("Fallo conexion con bd")
            End If


        Catch ex As Exception
            Throw New Exception("Error, Fallo conexión a base de datos")

        End Try

    End Function
    Public Function TieneVariosTnu(ByVal Conexion As String, ByVal Sec As Integer) As Integer
        Dim con As System.Data.SqlClient.SqlConnection
        Dim reader As System.Data.SqlClient.SqlDataReader
        Dim sql As System.Data.SqlClient.SqlCommand

        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = Conexion


        Try
            con.Open()

            sql = New System.Data.SqlClient.SqlCommand
            sql.CommandText = " TieneVariosTNU " & Sec


            sql.Connection = con

            reader = sql.ExecuteReader()
            reader.Read.ToString()

            TieneVariosTnu = reader.Item(0)

            reader.Close()
            con.Close()
        Catch er As Exception
            MessageBox(Err.Description)
            reader.Close()
            con.Close()
        End Try

    End Function
    Public Function InicioFecha(ByVal Conexion As String, ByVal lug As Integer, ByVal tnu As String) As Date
        Dim con As System.Data.SqlClient.SqlConnection
        Dim reader As System.Data.SqlClient.SqlDataReader
        Dim sql As System.Data.SqlClient.SqlCommand

        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = Conexion


        Try
            con.Open()

            sql = New System.Data.SqlClient.SqlCommand
            sql.CommandText = " PrimerDiaDisponible " & lug & "," & tnu


            sql.Connection = con

            reader = sql.ExecuteReader()
            reader.Read.ToString()

            InicioFecha = reader.Item(0)

            reader.Close()
            con.Close()
        Catch er As Exception
            MessageBox(Err.Description)
        End Try

    End Function
    Function EsFeriado(ByVal fecha As Date, ByVal Conexion As String) As Boolean

        Dim con As System.Data.SqlClient.SqlConnection
        Dim reader As System.Data.SqlClient.SqlDataReader
        Dim sql As System.Data.SqlClient.SqlCommand
        Dim sSql As String

        sSql = "  Select count(Fer_Fecha) from Feriados where "
        sSql = sSql & " Fer_Fecha = '" & fecha & "'"

        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = Conexion


        Try
            con.Open()

            sql = New System.Data.SqlClient.SqlCommand
            sql.CommandText = sSql


            sql.Connection = con

            reader = sql.ExecuteReader()
            reader.Read.ToString()


            If reader.Item(0) > 0 Then
                EsFeriado = True

            Else
                EsFeriado = False
            End If



            reader.Close()
            con.Close()
        Catch er As Exception
            'MessageBox(Err.Description)
        End Try

    End Function

    Public Function MesDescripcion(ByVal mes As Byte)
        MesDescripcion = ""
        Select Case mes
            Case 1
                MesDescripcion = "Enero"
            Case 2
                MesDescripcion = "Febrero"
            Case 3
                MesDescripcion = "Marzo"
            Case 4
                MesDescripcion = "Abril"
            Case 5
                MesDescripcion = "Mayo"
            Case 6
                MesDescripcion = "Junio"
            Case 7
                MesDescripcion = "Julio"
            Case 8
                MesDescripcion = "Agosto"
            Case 9
                MesDescripcion = "Septiembre"
            Case 10
                MesDescripcion = "Octubre"
            Case 11
                MesDescripcion = "Noviembre"
            Case 12
                MesDescripcion = "Diciembre"
        End Select
    End Function
    Sub DeterminaHorariosDiaCombo(ByVal Fecha As DateTime, ByVal Lugar As Integer, ByVal TipoNum As String, ByVal combo As DropDownList, ByVal calend As Calendar)

        combo.Items.Clear()
        CargoComboStore(combo, "DeterminaHorariosxDiayLugaryTipoNumero " & Year(Fecha) & "," & Month(Fecha) & "," & Day(Fecha) & "," & Lugar & ",'" & TipoNum & "','" & Fecha & "'", "", ConexionGT)

        If combo.Items.Count = 0 Then
            combo.Items.Clear()
            combo.Items.Add(New ListItem("Sin turnos"))
        End If

    End Sub
    Sub DeterminaHorariosDiaComboConSobreturno(ByVal Fecha As DateTime, ByVal Lugar As Integer, ByVal TipoNum As String, ByVal combo As DropDownList, ByVal calend As Calendar)

        combo.Items.Clear()
        CargoComboStore(combo, "DeterminaHorariosxDiayLugaryTipoNumeroConSobreturno " & Year(Fecha) & "," & Month(Fecha) & "," & Day(Fecha) & "," & Lugar & ",'" & TipoNum & "','" & Fecha & "'", "", ConexionGT)
        CargoDsStore(combo, "DeterminaHorariosxDiayLugaryTipoNumero " & Year(Fecha) & "," & Month(Fecha) & "," & Day(Fecha) & "," & Lugar & ",'" & TipoNum & "','" & Fecha & "'", "", ConexionGT)
        If combo.Items.Count = 0 Then
            combo.Items.Clear()
            combo.Items.Add(New ListItem("Sin turnos"))
        End If

    End Sub
    Function LeyendaTipoTramite(ByVal cod As Integer, ByVal Conexion As String) As String
        Dim con As System.Data.SqlClient.SqlConnection
        Dim reader As System.Data.SqlClient.SqlDataReader
        Dim sql As System.Data.SqlClient.SqlCommand
        Dim sSql As String

        sSql = "  Select STNU_Detalle from SubTipos_Numeros where "
        sSql = sSql & " STNU_Codigo = " & cod

        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = Conexion


        Try
            con.Open()

            sql = New System.Data.SqlClient.SqlCommand
            sql.CommandText = sSql


            sql.Connection = con

            reader = sql.ExecuteReader()
            reader.Read.ToString()


            If reader.Item(0) <> "" Then
                LeyendaTipoTramite = reader.Item(0).ToString
            Else
                LeyendaTipoTramite = ""
            End If



            reader.Close()
            con.Close()
        Catch er As Exception
            MessageBox(Err.Description)
        End Try

    End Function
    Public Function TopeFecha(ByVal Conexion As String, ByVal lug As Integer) As Date
        Dim con As System.Data.SqlClient.SqlConnection
        Dim reader As System.Data.SqlClient.SqlDataReader
        Dim sql As System.Data.SqlClient.SqlCommand

        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = Conexion


        Try
            con.Open()

            sql = New System.Data.SqlClient.SqlCommand
            sql.CommandText = " UltimoDiaDisponible " & lug


            sql.Connection = con

            reader = sql.ExecuteReader()
            reader.Read.ToString()

            TopeFecha = reader.Item(0)

            reader.Close()
            con.Close()
        Catch er As Exception
            MessageBox(Err.Description)
        End Try

    End Function
    Public Function LinkWeb(ByVal stnu As String, ByVal Conexion As String) As String
        Dim con As System.Data.SqlClient.SqlConnection
        Dim reader As System.Data.SqlClient.SqlDataReader
        Dim sql As System.Data.SqlClient.SqlCommand

        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = Conexion



        Try
            con.Open()

            sql = New System.Data.SqlClient.SqlCommand
            sql.CommandText = " Select STNU_LinkWeb "
            sql.CommandText = sql.CommandText & " from SubTipos_Numeros "
            sql.CommandText = sql.CommandText & " where "
            sql.CommandText = sql.CommandText & " STNU_Codigo = " & stnu

            sql.Connection = con

            reader = sql.ExecuteReader()
            reader.Read.ToString()

            LinkWeb = reader.Item(0).ToString

            reader.Close()
            con.Close()
        Catch er As Exception
            MessageBox(Err.Description.ToString)
        End Try


    End Function
    Function horadesdehasta(ByVal diasemana As Integer) As String
        Dim sql As String

        sql = " Select datepart(hh,DYH_HoraDesde), datepart(hh,DYH_HoraHasta),"
        sql = sql & " DYH_TurnosxHora, DYH_FechaModificacion "
        sql = sql & " from  Dias_y_Horarios "
        sql = sql & " where "
        sql = sql & " DYH_Habilitado = 1 "
        sql = sql & " and  "
        sql = sql & " DYH_Dia = " & diasemana

        horadesdehasta = sql

    End Function
    Function TieneCV(ByVal numdoc As Integer, ByVal Conexion As String) As Boolean
        Dim con As System.Data.SqlClient.SqlConnection
        Dim reader As System.Data.SqlClient.SqlDataReader
        Dim sql As System.Data.SqlClient.SqlCommand
        Dim sSql As String

        sSql = "  Select Count(*) as Tiene from Personas where "
        sSql = sSql & " PER_Documento = " & numdoc

        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = Conexion


        Try
            con.Open()

            sql = New System.Data.SqlClient.SqlCommand
            sql.CommandText = sSql


            sql.Connection = con

            reader = sql.ExecuteReader()
            reader.Read.ToString()


            If reader.Item(0) > 0 Then
                TieneCV = True
            Else
                TieneCV = False
            End If



            reader.Close()
            con.Close()
        Catch er As Exception
            MessageBox(Err.Description)
        End Try

    End Function
    Public Sub DeterminaTipoyNumeroxSubtipodeNumero(ByVal stn As String, ByVal Conexion As String)
        Dim con As System.Data.SqlClient.SqlConnection
        Dim reader As System.Data.SqlClient.SqlDataReader
        Dim sql As System.Data.SqlClient.SqlCommand

        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = Conexion

        Try
            con.Open()

            sql = New System.Data.SqlClient.SqlCommand
            sql.CommandText = " Select SEC_Numero, TNU_Codigo "
            sql.CommandText = sql.CommandText & " from SubTipos_Numeros "
            sql.CommandText = sql.CommandText & " where "
            sql.CommandText = sql.CommandText & " STNU_Codigo = " & stn

            sql.Connection = con
            reader = sql.ExecuteReader()
            reader.Read.ToString()

            Session("NroSec") = reader.Item(0)
            Session("TipNro") = reader.Item(1)


            reader.Close()
            con.Close()
        Catch er As Exception
            MessageBox(Err.Description.ToString)
        End Try

    End Sub

    Public Function PoneTopeFecha(ByVal Conexion As String)
        Dim con As System.Data.SqlClient.SqlConnection
        Dim reader As System.Data.SqlClient.SqlDataReader
        Dim sql As System.Data.SqlClient.SqlCommand

        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = Conexion
        PoneTopeFecha = ""


        Try
            con.Open()

            sql = New System.Data.SqlClient.SqlCommand
            sql.CommandText = " Select day(getdate() + PTI_DiasMaximoSacarNumero), "
            sql.CommandText = sql.CommandText & " month(getdate() + PTI_DiasMaximoSacarNumero),"
            sql.CommandText = sql.CommandText & " year(getdate() + PTI_DiasMaximoSacarNumero) "
            sql.CommandText = sql.CommandText & " from ParametrosSacaNrosInternet"

            sql.Connection = con
            reader = sql.ExecuteReader()
            reader.Read.ToString()

            PoneTopeFecha = reader.Item(0) & " de " & MesDescripcion(reader.Item(1)) & " de " & reader.Item(2)


            reader.Close()
            con.Close()
        Catch er As Exception
            MessageBox(Err.Description.ToString)
        End Try


    End Function




    Function DireccionLugar(ByVal lugar As Integer, ByVal Conexion As String) As String
        Dim con As System.Data.SqlClient.SqlConnection
        Dim reader As System.Data.SqlClient.SqlDataReader
        Dim sql As System.Data.SqlClient.SqlCommand
        Dim sSql As String

        sSql = "  Select LUG_Dirección from LugarCabecera lc "
        sSql = sSql & " inner join Sectores s "
        sSql = sSql & " on lc.LUG_Numero = s.LUG_Numero "
        sSql = sSql & " where "
        sSql = sSql & " s.SEC_Numero = " & lugar

        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = Conexion


        Try
            con.Open()

            sql = New System.Data.SqlClient.SqlCommand
            sql.CommandText = sSql


            sql.Connection = con

            reader = sql.ExecuteReader()
            reader.Read.ToString()


            If reader.Item(0) <> "" Then
                DireccionLugar = reader.Item(0).ToString
            Else
                DireccionLugar = ""
            End If



            reader.Close()
            con.Close()
        Catch er As Exception
            MessageBox(Err.Description)
        End Try

    End Function

    Public Function FechaInicioLS(ByVal Conexion As String) As Date
        Dim con As System.Data.SqlClient.SqlConnection
        Dim reader As System.Data.SqlClient.SqlDataReader
        Dim sql As System.Data.SqlClient.SqlCommand

        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = Conexion
        '        TopeFecha = ""

        Try
            con.Open()

            sql = New System.Data.SqlClient.SqlCommand
            sql.CommandText = " Select PTI_FechaInicioLS "
            sql.CommandText = sql.CommandText & " from ParametrosSacaNrosInternet"

            sql.Connection = con

            reader = sql.ExecuteReader()
            reader.Read.ToString()

            FechaInicioLS = reader.Item(0)


            reader.Close()
            con.Close()
        Catch er As Exception
            MessageBox(Err.Description.ToString)
        End Try


    End Function

    Public Function FechaInicio(ByVal Conexion As String) As Date
        Dim con As System.Data.SqlClient.SqlConnection
        Dim reader As System.Data.SqlClient.SqlDataReader
        Dim sql As System.Data.SqlClient.SqlCommand

        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = Conexion
        '        TopeFecha = ""

        Try
            con.Open()

            sql = New System.Data.SqlClient.SqlCommand
            sql.CommandText = " Select PTI_FechaInicio "
            sql.CommandText = sql.CommandText & " from ParametrosSacaNrosInternet"

            sql.Connection = con

            reader = sql.ExecuteReader()
            reader.Read.ToString()

            FechaInicio = reader.Item(0)


            reader.Close()
            con.Close()
        Catch er As Exception
            MessageBox(Err.Description.ToString)
        End Try


    End Function
    Sub CargoCombo(ByVal cbo As DropDownList, ByVal sql As String, ByVal sconexion As String)
        Dim dll As New FuncionesBasicas.Funciones
        Dim con As Data.SqlClient.SqlConnection
        Dim ds As Data.DataSet
        Dim dahorarios As Data.SqlClient.SqlDataAdapter

        cbo.Items.Clear()

        Try

            'Breve resumen explicado de pasos a seguir
            '1-Se establece el objeto conexión 
            '2-Se abre la conexión 
            con = New Data.SqlClient.SqlConnection(sconexion)
            con.Open()


            '3-Se escribe la consulta sql en variable tipo string
            'sql = dll.

            '4-El Objeto SqlAdapter se ejecuta en la conexión abierta
            'con la cadena sql de tipo string
            dahorarios = New Data.SqlClient.SqlDataAdapter(sql, con)
            '5-Se define un objeto dataset para volcar el resultado
            ds = New Data.DataSet("XXX")
            '6-Se descarga el resultado contenido en el objeto SQLAdapter al Dataset
            'con el metodo Fill
            dahorarios.Fill(ds)
            'dahorarios.FillSchema(ds, SchemaType.Source)

            '7-Carga el combo con mierda 
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    For Each dr As Data.DataRow In ds.Tables(0).Rows
                        cbo.Items.Add(New ListItem(dr.Item(0), dr.Item(1)))
                    Next
                End If
            End If
            con.Close()

            'reader.Close()
        Catch er As Exception
            MsgBox(Err.Description)
        End Try

    End Sub
    Public Sub EstadoDeConexion(con As System.Data.SqlClient.SqlConnection)
        If con.State = 1 Then
            con.Close()
        End If
    End Sub
    Public Sub EstadoDeConexion(reader As System.Data.SqlClient.SqlDataReader)
        If reader.IsClosed = False Then
            reader.Close()
        End If
    End Sub

#End Region

End Class
