Imports CrystalDecisions.CrystalReports.Engine 'agregado 17/11/2015 by Wil
Imports CrystalDecisions.Shared 'agregado 17/11/2015 by Wil 
Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net
Imports System.Net.Mail
Imports System.Web.UI.HtmlControls.HtmlInputText

Partial Class EntregaTurnos
    'Inherits System.Web.UI.Page
    Inherits Base
    Dim _errorLog As ErrorLog = New ErrorLog()

#Region "Botones"


    Protected Sub calFechaTurno_DayRender(sender As Object, e As DayRenderEventArgs) Handles calFechaTurno.DayRender
        Try
            If e.Day.IsSelected Then
                e.Cell.BackColor = Drawing.Color.DarkSlateGray
            End If
        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                If (Session.Count < 1) Then
                    Response.Redirect("Default.aspx?Mensaje=" & 1)
                End If
            End If

            Dim sql As String

            If Session("Lugar") Is Nothing Then
                Session("Lugar") = "2"
            End If
            lblLugar.Text = Session("Lugar").ToString
            lblTipoNum.Text = Session("Numero").ToString

            If ddlSubTipoNumero.Items.Count > 0 Then
                Me.txtdetalletramite.Text = LeyendaTipoTramite(Me.ddlSubTipoNumero.SelectedValue, ConexionGT)
            End If

            If Not Page.IsPostBack Then

                calFechaTurno.SelectedDates.Clear()
                calFechaTurno.VisibleDate = InicioFecha(ConexionGT, Session("Lugar"), Session("Numero"))
                calFechaTurno.SelectedDate = InicioFecha(ConexionGT, Session("Lugar"), Session("Numero"))
                calFechaTurno.SelectedDates.Add(InicioFecha(ConexionGT, Session("Lugar"), Session("Numero")))





                lblFecTopEN.Text = TopeFecha(ConexionGT, Session("Lugar"))

                'Me.lblFechaInicio.Text = Now().Date
                'lblFecTopEN.Text = Now().Date

                Dim dFechaInit As Date
                dFechaInit = InicioFecha(ConexionGT, Session("Lugar"), Session("Numero"))
                Me.lblFechaInicio.Text = dFechaInit


                Dim fi As DateTime = lblFechaInicio.Text
                Dim fh As DateTime = lblFecTopEN.Text


                If dFechaInit = "#12:00:00 AM#" Then
                    Me.Leyendainicio.Visible = False
                    Me.LeyendaFin.Visible = False
                    Me.lblmensaje.Text = "No hay turnos disponibles"
                    Me.lblFecTopEN.Visible = False
                    Me.lblFechaInicio.Visible = False
                    Session("Lugar") = "999"
                    Response.Redirect("inicioTurnos.aspx")
                Else
                    Me.lblmensaje.Text = "Seleccione Horario"
                    Me.Leyendainicio.Visible = True
                    Me.LeyendaFin.Text = "hasta el día :"
                    Me.lblFecTopEN.Visible = True
                    Me.lblFechaInicio.Visible = True
                End If


                'Comentado 05/06/2017 Die
                'Me.calFechaTurno.SelectedDate = Now()
                'lblFechaSeleccionada.Text = Day(calFechaTurno.SelectedDate) & " de " & MesDescripcion(Month(calFechaTurno.SelectedDate)) & " de " & Year(calFechaTurno.SelectedDate)

                Me.ddlSubTipoNumero.Items.Clear()


                CargoComboStore(Me.ddlSubTipoNumero, "SubTiposNumerosxLugarySectoryTnu " & lblLugar.Text & ", " & lblTipoNum.Text & " ", "", ConexionGT)
                Me.txtdetalletramite.Text = LeyendaTipoTramite(Me.ddlSubTipoNumero.SelectedValue, ConexionGT)
                Me.HPLDocumentacion.NavigateUrl = LinkWeb(Me.ddlSubTipoNumero.SelectedValue, ConexionGT)


                'CargoComboStore(Me.ddlSubTipoNumero, "SubTiposNumerosxLugarySectoryTnu " & Session("Lugar") & ", " & Session("Numero") & " ", "", ConexionGT)


                cmdConfirmaTurno1.Visible = False

                Me.ddlTipoDoc.Items.Clear()
                Me.ddlTipoDoc.Items.Add(New ListItem("DNI", 0))
                Me.ddlTipoDoc.Items.Add(New ListItem("LE", 1))
                Me.ddlTipoDoc.Items.Add(New ListItem("LC", 2))
                Me.ddlTipoDoc.Items.Add(New ListItem("CIPF", 3))
                Me.ddlTipoDoc.Items.Add(New ListItem("CIRPF", 4))
                Me.ddlTipoDoc.Items.Add(New ListItem("OTR", 5))

                'Pregunto si es feriado
                If EsFeriado(calFechaTurno.SelectedDate.Date, ConexionGT) Then
                    lblmens.Text = "La Fecha Seleccionada es Feriado, por favor seleccione otra fecha"
                    lblFechaSeleccionada.Text = ""
                    cmdConfirmaTurno1.Visible = False
                    Exit Sub
                End If

                If calFechaTurno.SelectedDate < Now().Date Or calFechaTurno.SelectedDate > Me.lblFecTopEN.Text Then
                    lblmens.ForeColor = Drawing.Color.Gray
                    ddlturnosDisponibles.Items.Clear()
                    ddlturnosDisponibles.Items.Add(New ListItem("Sin turnos"))
                    lblmens.Text = "Solo puede sacar turnos para días posteriores al actual !"
                    lblFechaSeleccionada.Text = ""
                    cmdConfirmaTurno1.Visible = False
                    Exit Sub
                Else
                    lblFechaSeleccionada.ForeColor = Drawing.Color.Gray
                    lblFechaSeleccionada.Text = Day(calFechaTurno.SelectedDate) & " de " & MesDescripcion(Month(calFechaTurno.SelectedDate)) & " de " & Year(calFechaTurno.SelectedDate)
                    lblmens.Text = ""
                    cmdConfirmaTurno1.Visible = True
                    Dim hora As String = Right(Now(), 8)

                    If Session("SobreTurno") = True Then
                        DeterminaHorariosDiaComboConSobreturno(calFechaTurno.SelectedDate.Date, Session("Lugar"), Session("Numero"), Me.ddlturnosDisponibles, calFechaTurno)
                    Else
                        DeterminaHorariosDiaCombo(calFechaTurno.SelectedDate.Date, Session("Lugar"), Session("Numero"), Me.ddlturnosDisponibles, calFechaTurno)
                    End If
                End If

                Me.lblerror.Text = ""

            Else

                Me.lblerror.Text = ""
                Dim a As String = Now().Date

                If EsFeriado(calFechaTurno.SelectedDate.Date, ConexionGT) Then
                    lblmens.Text = "La Fecha Seleccionada es Feriado, por favor seleccione otra fecha"
                    ddlturnosDisponibles.Items.Clear()
                    ddlturnosDisponibles.Items.Add(New ListItem("Sin turnos"))
                    cmdConfirmaTurno1.Visible = False
                    Exit Sub
                End If
                'Or SacaTurnoDiaActual(ConexionGT, Session("Lugar")) 
                If calFechaTurno.SelectedDate < Now().Date Or calFechaTurno.SelectedDate > Me.lblFecTopEN.Text Then
                    lblmens.ForeColor = Drawing.Color.Gray
                    ddlturnosDisponibles.Items.Clear()
                    ddlturnosDisponibles.Items.Add(New ListItem("Sin turnos"))
                    lblmens.Text = "Solo puede sacar turnos para días posteriores al actual !"
                    lblFechaSeleccionada.Text = ""
                    cmdConfirmaTurno1.Visible = False
                    Exit Sub
                Else
                    lblFechaSeleccionada.ForeColor = Drawing.Color.Gray
                    lblFechaSeleccionada.Text = Day(calFechaTurno.SelectedDate) & " de " & MesDescripcion(Month(calFechaTurno.SelectedDate)) & " de " & Year(calFechaTurno.SelectedDate)
                    lblmens.Text = ""
                    cmdConfirmaTurno1.Visible = True
                    Dim hora As String = Right(Now(), 8)

                    If Session("SobreTurno") = True Then
                        DeterminaHorariosDiaComboConSobreturno(calFechaTurno.SelectedDate.Date, Session("Lugar"), Session("Numero"), Me.ddlturnosDisponibles, calFechaTurno)
                    Else
                        DeterminaHorariosDiaCombo(calFechaTurno.SelectedDate.Date, Session("Lugar"), Session("Numero"), Me.ddlturnosDisponibles, calFechaTurno)
                    End If
                End If

                Me.lblerror.Text = ""
            End If




            Me.ddlTipoDoc.Items.Clear()
            Me.ddlTipoDoc.Items.Add(New ListItem("DNI", 0))
            Me.ddlTipoDoc.Items.Add(New ListItem("LE", 1))
            Me.ddlTipoDoc.Items.Add(New ListItem("LC", 2))
            Me.ddlTipoDoc.Items.Add(New ListItem("CIPF", 3))
            Me.ddlTipoDoc.Items.Add(New ListItem("CIRPF", 4))
            Me.ddlTipoDoc.Items.Add(New ListItem("OTR", 5))
        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try


    End Sub

    Protected Sub calFechaTurno_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calFechaTurno.SelectionChanged
        Try
            If EsFeriado(calFechaTurno.SelectedDate, ConexionGT) Then
                lblmens.Text = "La Fecha Seleccionada es Feriado, por favor seleccione otra fecha"
                ddlturnosDisponibles.Items.Clear()
                lblFechaSeleccionada.Text = ""
                ddlturnosDisponibles.Items.Add(New ListItem("Sin turnos"))
                cmdConfirmaTurno1.Visible = False
                Exit Sub
            End If

            'If calFechaTurno.SelectedDate < FechaInicio(ConexionGT) Or calFechaTurno.SelectedDate > TopeFecha(ConexionGT) Or calFechaTurno.SelectedDate <= Now() Then
            '
            If calFechaTurno.SelectedDate < Now().Date Or calFechaTurno.SelectedDate > Me.lblFecTopEN.Text Then
                lblmens.ForeColor = Drawing.Color.Gray
                ddlturnosDisponibles.Items.Clear()
                ddlturnosDisponibles.Items.Add(New ListItem("Sin turnos"))
                lblmens.Text = "Solo puede sacar turnos para días posteriores al actual !"
                lblFechaSeleccionada.Text = ""
                cmdConfirmaTurno1.Visible = False
                Exit Sub
            Else
                lblFechaSeleccionada.ForeColor = Drawing.Color.Gray
                lblFechaSeleccionada.Text = Day(calFechaTurno.SelectedDate) & " de " & MesDescripcion(Month(calFechaTurno.SelectedDate)) & " de " & Year(calFechaTurno.SelectedDate)
                lblmens.Text = ""
                cmdConfirmaTurno1.Visible = True
                Dim hora As String = Right(Now(), 8)



                If Session("SobreTurno") = True Then
                    DeterminaHorariosDiaComboConSobreturno(calFechaTurno.SelectedDate.Date, Session("Lugar"), Session("Numero"), Me.ddlturnosDisponibles, calFechaTurno)
                Else
                    DeterminaHorariosDiaCombo(calFechaTurno.SelectedDate.Date, Session("Lugar"), Session("Numero"), Me.ddlturnosDisponibles, calFechaTurno)
                End If



            End If
        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try
    End Sub



    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            EnviarMailPrueba("", "die9suarez@gmail.com", "", "")
        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try
    End Sub


    Protected Sub cmdConfirmaTurno1_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdConfirmaTurno1.ServerClick
        Try
            Dim reporte As New ReportDocument
            Dim Reserva As String
            Dim dahorarios As New SqlDataAdapter
            Dim bconfirmado As Integer
            Dim dredid As System.Data.SqlClient.SqlDataReader
            Dim dredreserva As System.Data.SqlClient.SqlDataReader
            Dim Documento As String
            Dim CodigoBaja As String
            Dim ssql As String
            Dim ds As DataSet
            Dim insertado As Boolean
            If DatosValidos() Then

                DeterminaTipoyNumeroxSubtipodeNumero(Me.ddlSubTipoNumero.SelectedValue, ConexionGT)
                dredid = SqlHelper.ExecuteReader(ConexionGT, System.Data.CommandType.Text, "IDLibre '" & Me.calFechaTurno.SelectedDate.Date & " " & Me.ddlturnosDisponibles.SelectedItem.Text & "'," & Session("Lugar") & "," & Session("TipNro") & "")
                dredid.Read()

                If dredid.Item(0).ToString <> "" Then

                    Reserva = dredid.Item(0).ToString
                    Documento = txtDocumento.Value
                    CodigoBaja = Left(Reserva, 3) & Documento

                    bconfirmado = 1 'modificado, para entregar el número ya confirmado poner en 1

   





                        dredreserva = SqlHelper.ExecuteReader(ConexionGT, CommandType.StoredProcedure, "SP_PrimerReservaDeTurnoNuevo", _
                        New SqlParameter("@stnu", Me.ddlSubTipoNumero.SelectedValue), _
                        New SqlParameter("@tdni", Me.ddlTipoDoc.SelectedItem.Text), _
                        New SqlParameter("@doc", Me.txtDocumento.Value), _
                        New SqlParameter("@mail", Me.txtmail.Text), _
                        New SqlParameter("@AtenV", Me.txtDoc0800.Text), _
                        New SqlParameter("@ApNom", Me.txtApNom.Text), _
                        New SqlParameter("@TelC", Me.txtTelContacto.Text), _
                        New SqlParameter("@conf", bconfirmado), _
                        New SqlParameter("@ID", Reserva))
                        dredreserva.Read()

                        


                        If dredreserva.Item(0) = 0 Then
                            lblmens.Text = "El turno seleccionado no pudo ser ingresado, reinténtelo !"
                            Exit Sub
                        Else
                            insertado = True
                        End If



                    Else
                        dredid = SqlHelper.ExecuteReader(ConexionGT, System.Data.CommandType.Text, "IDSobreTurno '" & Me.calFechaTurno.SelectedDate.Date & " " & Me.ddlturnosDisponibles.SelectedItem.Text & "'," & Session("Lugar") & "," & Session("TipNro") & "")
                        dredid.Read()

                        Reserva = dredid.Item(0).ToString
                        Documento = txtDocumento.Value
                        CodigoBaja = Left(Reserva, 3) & Documento

                        bconfirmado = 1 'modificado, para entregar el número ya confirmado poner en 1

                        dredreserva = SqlHelper.ExecuteReader(ConexionGT, CommandType.StoredProcedure, "SP_PrimerReservaDeTurnoNuevoSobreturno", _
                              New SqlParameter("@stnu", Me.ddlSubTipoNumero.SelectedValue), _
                              New SqlParameter("@tdni", Me.ddlTipoDoc.SelectedItem.Text), _
                              New SqlParameter("@doc", Me.txtDocumento.Value), _
                              New SqlParameter("@mail", Me.txtmail.Text), _
                              New SqlParameter("@AtenV", Session("Dni").ToString), _
                              New SqlParameter("@ApNom", Me.txtApNom.Text), _
                              New SqlParameter("@TelC", Me.txtTelContacto.Text), _
                              New SqlParameter("@conf", bconfirmado), _
                              New SqlParameter("@ID", Reserva))

                        dredreserva.Read()

                        Reserva = dredreserva.Item(0).ToString
                        CodigoBaja = Left(Reserva, 3) & Documento
                        insertado = True

                    End If

                    'Dim sLugDesc = DireccionLugar(lblLugar.Text, ConexionGT)
                    'EnviarMailConfirmado(Reserva, Me.txtmail.Text, sLugDesc, Documento, CodigoBaja)

                    'Session("mens0") = "Estimado/a: " & txtApNom.Text.ToString & " ."
                    'Session("mens1") = "Se ha confirmado el siguiente turno "
                    'Session("mens2") = "Trámite: " & Me.ddlSubTipoNumero.SelectedItem.ToString & ". "
                    'Session("mens3") = "Fecha del Turno: " & lblFechaSeleccionada.Text.ToString & ",  " & ddlturnosDisponibles.SelectedItem.ToString & "hs. "
                    'Session("mens4") = "Dirección: " & sLugDesc
                    'Session("mens5") = "Podrá anular su turno utilizando el siguiente código. "
                    'Session("mens6") = "Código de anulación: "
                    'Session("mens10") = "" & CodigoBaja
                    'Session("mens7") = "Una vez cancelado podrá solicitar uno nuevo."
                    'Session("mens8") = "RECUERDE tomar nota o guardar su Código de Anulación. "
                    'Session("mens9") = "Gracias por utilizar nuestros servicios "
                    'Session("mens11") = ""

                    'DeterminaHorariosDiaCombo(calFechaTurno.SelectedDate.Date, lblLugar.Text, lblTipoNum.Text, Me.ddlturnosDisponibles, calFechaTurno)

                    'Me.txtmail.Text = ""
                    'Me.txtrepitemail.Text = ""
                    'Me.txtDocumento.Value = ""
                    'Me.txtApNom.Text = ""
                    'Me.txtTelContacto.Text = ""
                    'dredid.Close()
                    'dredreserva.Close()

                    ''If lblLugar.Text = 4 Then
                    ''    Response.Redirect("TramitesUno.aspx")
                    ''ElseIf lblLugar.Text = 33 Then
                    ''    'Response.Redirect("RequisitosCDR.aspx")
                    ''    Response.Redirect("mensajeturno.aspx")
                    ''ElseIf lblLugar.Text = 65 Then
                    ''    Session("mens11") = "~/img/Zoo.jpg"
                    ''    'Session("mens11") = "ImageUrl = ~/img/Zoo.jpg"
                    ''    Response.Redirect("mensajeturno.aspx")
                    ''    'Response.Redirect("RequisitosZoo.aspx")
                    ''Else
                    ''    Response.Redirect("mensajeturno.aspx")
                    ''End If

                    If insertado = True Then
                        Response.Redirect("Principal.aspx?Mensaje=" & 3 & CodigoBaja)
                    End If
                End If
        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try
    End Sub

    Protected Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Try
            Response.Redirect("principal.aspx")
        Catch ex As Exception
            _errorLog.LogError(ex)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try
    End Sub



#Region "Cerrar Sesion"
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
#End Region

#Region "Metodos"
    Public Sub TurnoTomadoxDocumentoInfo(ByVal hoy As String, ByVal hasta As String, ByVal doc As String)


        Dim con As System.Data.SqlClient.SqlConnection
        Dim reader As System.Data.SqlClient.SqlDataReader
        Dim sql As System.Data.SqlClient.SqlCommand


        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = ConexionGT()


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
            Me.lblmens.Text = "El Nº de Doc :" & reader.Item(0).ToString & " ya posee un turno asignado "
        End If

        reader.Close()
        con.Close()


    End Sub

    Public Function TurnoTomadoxDocumento(ByVal hoy As String, ByVal hasta As String, ByVal doc As String, ByVal Lugar As Integer) As Boolean


        Dim con As System.Data.SqlClient.SqlConnection
        Dim reader As System.Data.SqlClient.SqlDataReader
        Dim sql As System.Data.SqlClient.SqlCommand


        TurnoTomadoxDocumento = False
        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = ConexionGT()


        con.Open()


        sql = New System.Data.SqlClient.SqlCommand

        sql.CommandText = " Select Count(*) as TieneTurno from Turnos "
        sql.CommandText = sql.CommandText & " where "
        sql.CommandText = sql.CommandText & " TUR_Documento = '" & doc & "' and "
        sql.CommandText = sql.CommandText & " SEC_Numero = " & Lugar & " and "
        sql.CommandText = sql.CommandText & " TUR_FechaTurno >= '" & hoy & "' and "
        sql.CommandText = sql.CommandText & " TUR_FechaTurno <= '" & hasta & "'"
        sql.CommandText = sql.CommandText & " and [TUR_BajaLogica] is null"

        sql.Connection = con
        reader = sql.ExecuteReader()
        reader.Read.ToString()


        If reader.Item(0) >= 1 Then
            TurnoTomadoxDocumento = True
        End If

        reader.Close()
        con.Close()
   
    End Function
    Public Function DiaHabilitadoenTabla(ByVal Fecha As Date, ByVal Lugar As Integer) As Boolean

        Dim con As System.Data.SqlClient.SqlConnection
        Dim reader As System.Data.SqlClient.SqlDataReader
        Dim sql As System.Data.SqlClient.SqlCommand


        DiaHabilitadoenTabla = False
        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = ConexionGT()



        con.Open()


        sql = New System.Data.SqlClient.SqlCommand
        sql.CommandText = " Select Count(*) as Existe from  Dias_y_Horarios "
        sql.CommandText = sql.CommandText & " where "
        sql.CommandText = sql.CommandText & " DYH_Habilitado = 1 and  "
        sql.CommandText = sql.CommandText & " LUG_Numero =" & Lugar & " and "
        sql.CommandText = sql.CommandText & " DYH_Dia = " & Weekday(Fecha)

        sql.Connection = con
        reader = sql.ExecuteReader()
        reader.Read.ToString()


        If reader.Item(0) >= 1 Then
            DiaHabilitadoenTabla = True
        End If


        reader.Close()
        con.Close()
   

    End Function
    Sub DiaHabilitado1()
        Dim Cnn, rs

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
            'ElseIf 60 / txh > 5 And 60 / txh <= 10 Then
            '    interv = 10
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
    Sub DeterminaParametrosCombo(ByVal Fecha As Date, ByVal Lugar As Integer)
        Dim con As SqlConnection
        Dim sql As String
        Dim ds As DataSet
        Dim dahorarios As SqlDataAdapter
        Dim dst As DataSet
        Dim daturnostomados As SqlDataAdapter
        Dim hi As Integer
        Dim hf, hfo As Integer
        Dim MINHF, MINHI As Integer
        Dim horfinpart As Boolean
        Dim txh, ci, i, j As Integer
        Dim fmp As Date
        Dim interv As Integer
        Dim ttomado As Boolean
        Dim autoin As Long = 1

        Me.ddlturnosDisponibles.Items.Clear()



        'VALIDAR QUE EL TURNO SELECCIONADO AUN NO HAYA SIDO TOMADO 
        'POR OTRO USUARIO EN EL INTERIN DEL TRAMITE
        'Breve resumen explicado de pasos a seguir
        '1-Se establece el objeto conexión 
        '2-Se abre la conexión 
        con = New SqlConnection(ConexionGT)
        con.Open()

        '3-Se escribe la consulta sql en variable tipo string

        sql = " Select datepart(hh,DYH_HoraDesde),   datepart(hh,DYH_HoraHasta), "
        sql = sql & " DYH_TurnosxHora, DYH_FechaModificacion , datepart(mi,DYH_HoraHasta) , datepart(mi,DYH_HoraDesde) "
        sql = sql & " from Dias_y_Horarios "
        sql = sql & " where "
        sql = sql & " DYH_Habilitado = 1  and   "
        sql = sql & " LUG_Numero =" & Lugar & " and   "
        sql = sql & "  DYH_Dia = " & Weekday(Fecha)
        sql = sql & " order by datepart(hh,DYH_HoraDesde) "

        'sql = dll.horadesdehasta(Weekday(Fecha))
        'Comentado x Diego 02/06/2011
        'sql = dll.horadesdehasta(Weekday(Fecha))

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
        If ds.Tables(0).Rows.Count > 0 Then
            'Tomo la hora de comienzo
            For Each dr As DataRow In ds.Tables(0).Rows

                'horario inicio
                hi = dr.Item(0)
                MINHI = dr.Item(5)
                'horario finalización
                MINHF = dr.Item(4)
                If dr.Item(4) > 0 Then
                    hf = dr.Item(1) + 1
                    hfo = dr.Item(1)
                    horfinpart = True
                Else
                    hf = dr.Item(1)
                    horfinpart = False
                End If
                'personas . hora
                txh = dr.Item(2)

                'fecha de modificación del parámetro
                fmp = dr.Item(3)

                'Determina intervalos posibles
                interv = intervalos(txh)
                'Determina cantidad de intervalos en todo el rango horario
                If interv <> 0 Then
                    ci = 60 / interv
                End If


                'TURNOS TOMADOS--------------------------------------------------------------------------------------
                'Determina los turnos cargados en tabla turnos para el intervalo
                'para no mostrar los intervalos ocupados con turnos
                'GUARDO Cupos por Intervalo  

                If txh > 1 Then
                    Session("CXI") = txh / ci
                    'Session("CuposxIntervalo") = txh / (60 / interv)
                Else
                    Session("CXI") = txh
                End If

                'sql = dll.TurnosTomados(Year(calFechaTurno.SelectedDate), Month(calFechaTurno.SelectedDate), Day(calFechaTurno.SelectedDate), Session("CuposxIntervalo"))
                'SACA 

                sql = "  Select "
                sql = sql & " convert(varchar,TUR_FechaTurno,108) as HoraTurno,  "
                sql = sql & " TUR_FechaTurno "
                sql = sql & " from "
                sql = sql & " Turnos "
                sql = sql & " where "
                sql = sql & " year(TUR_FechaTurno) = " & Year(calFechaTurno.SelectedDate) & " and "
                sql = sql & " month(TUR_FechaTurno) = " & Month(calFechaTurno.SelectedDate) & " and "
                sql = sql & " Day(TUR_FechaTurno) = " & Day(calFechaTurno.SelectedDate) & " and "
                sql = sql & " LUG_Numero =" & Lugar
                sql = sql & " Group by "
                sql = sql & " TUR_FechaTurno "
                '(txh / interv) : CANT.de personas o turnos por intervalo.                    
                sql = sql & " having count(*) >= " & Session("CXI")

                daturnostomados = New SqlDataAdapter(sql, con)
                '5-Se define un objeto dataset para volcar el resultado
                dst = New DataSet("TT")
                '6-Se descarga el resultado contenido en el objeto SQLAdapter al Dataset
                'con el metodo Fill
                daturnostomados.Fill(dst)

                'Carga los intervalos respectivos en el combo
                For i = hi To hf
                    If ci < 60 Then
                        'valida que cargue uno menos para el rango TOTAl    
                        If i < hf Then
                            For j = 0 To 60 - interv Step interv
                                ttomado = False
                                If dst.Tables(0).Rows.Count > 0 Then
                                    For Each a As DataRow In dst.Tables(0).Rows
                                        'Cargo la puta matriz de tomados (turnos claro!)
                                        Dim caca As String
                                        caca = Format(i, "00") & ":" & Format(j, "00") & ":00"
                                        If a.Item(0).ToString = caca.ToString Then
                                            ttomado = True
                                            Exit For
                                        End If

                                    Next a
                                End If
                                ' si el turno no está tomado lo muestro en el combo

                                ' si el turno no está tomado lo muestro en el combo
                                If (hi = i And j >= MINHI) Or hi <> i Then
                                    If ttomado = False Then
                                        If horfinpart Then
                                            If hf - 1 = hfo Then
                                                If j < MINHF Then
                                                    Dim hora As String = Format(i, "00") & ":" & Format(j, "00")
                                                    autoin = autoin + 1
                                                    ddlturnosDisponibles.Items.Add(New ListItem(hora, Session("CXI").ToString & "-" & autoin.ToString))

                                                End If
                                            End If
                                        Else
                                            Dim hora As String = Format(i, "00") & ":" & Format(j, "00")
                                            autoin = autoin + 1
                                            ddlturnosDisponibles.Items.Add(New ListItem(hora, Session("CXI").ToString & "-" & autoin.ToString))
                                        End If

                                        'Dim hora As String = Format(i, "00") & ":" & Format(j, "00")
                                        'autoin = autoin + 1
                                        'ddlturnosDisponibles.Items.Add(New ListItem(hora, Session("CuposxIntervalo").ToString & "-" & autoin.ToString))
                                    End If
                                End If
                            Next j
                        End If
                    Else
                        ttomado = False
                        If dst.Tables(0).Rows.Count > 0 Then
                            For Each b As DataRow In dst.Tables(0).Rows
                                If b.Item(0) = Format(i, "00") & ":" & Format(j, "00") & ":00" Then
                                    ttomado = True
                                End If
                            Next b
                        End If

                        'valida que cargue uno menos para el rango TOTAl    
                        If i < hf Then
                            If ttomado = False Then
                                Dim hora As String = Format(i, "00") & ":" & Format(j, "00")
                                autoin = autoin + 1
                                ddlturnosDisponibles.Items.Add(New ListItem(hora, Session("CXI").ToString & "-" & autoin.ToString))
                            End If


                        End If
                    End If
                Next i



            Next
        End If

        con.Close()

        If Me.ddlturnosDisponibles.Items.Count = 0 Then
            ddlturnosDisponibles.Items.Clear()
            ddlturnosDisponibles.Items.Add(New ListItem("Sin turnos"))
        End If

  

    End Sub

    Sub CargoComboTurnosHorario(ByVal Fecha As Date)
        Dim con As SqlConnection
        Dim sql As String
        Dim ds As DataSet
        Dim dahorarios As SqlDataAdapter





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
                    ddlturnosDisponibles.Items.Add(New ListItem(dr.Item(2)))
                Next
            End If
        End If


        'reader.Close()
        con.Close()
   
    End Sub

    Sub CargoComboTipoTurno(ByVal cbo As DropDownList, ByVal sql As String, ByVal scondicion As String)
        Dim dll As New FuncionesBasicas.Funciones
        Dim con As SqlConnection
        Dim ds As DataSet
        Dim dahorarios As SqlDataAdapter

        cbo.Items.Clear()



        'Breve resumen explicado de pasos a seguir
        '1-Se establece el objeto conexión 
        '2-Se abre la conexión 
        con = New SqlConnection(dll.ConexionGT)
        con.Open()


        '3-Se escribe la consulta sql en variable tipo string
        'sql = dll.

        '4-El Objeto SqlAdapter se ejecuta en la conexión abierta
        'con la cadena sql de tipo string
        dahorarios = New SqlDataAdapter(sql, con)
        '5-Se define un objeto dataset para volcar el resultado
        ds = New DataSet("XXX")
        '6-Se descarga el resultado contenido en el objeto SQLAdapter al Dataset
        'con el metodo Fill
        dahorarios.Fill(ds)
        'dahorarios.FillSchema(ds, SchemaType.Source)

        '7-Carga el combo con mierda 
        If ds.Tables.Count > 0 Then
            If ds.Tables(0).Rows.Count > 0 Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    cbo.Items.Add(New ListItem(dr.Item(0), dr.Item(1)))
                Next
            End If
        End If

        con.Close()
        'reader.Close()


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


        If Me.ddlturnosDisponibles.Text = "Sin turnos" Then
            lblmens.Text = "No hay turnos disponibles para el día seleccionado !"
            DatosValidos = False
            Exit Function
        End If


        If Not validarEmail(Me.txtmail.Text) Then
            lblmens.Text = "complete correctamente el correo electrónico!"
            txtmail.Focus()
            DatosValidos = False
        End If

        If Me.txtmail.Text.ToUpper <> Me.txtrepitemail.Text.ToUpper Then
            lblmens.Text = "El mail ingresado no coincide con el original, Verifíquelo !!"
            Me.txtrepitemail.Focus()
            DatosValidos = False
        End If

        If Me.ddlSubTipoNumero.SelectedItem.Text = "" Then
            lblmens.Text = "Debe seleccionar el tipo de trámite !!"
            Me.ddlSubTipoNumero.Focus()
            DatosValidos = False
        End If


            If Not IsNumeric(Me.txtDocumento.Value) Then
                lblmens.Text = "complete correctamente el Nº de Documento"
                txtDocumento.Focus()
                DatosValidos = False
            End If

        'If Session("Lugar") = "4" And Val(Me.txtDocumento.Value) > 92000000 Then
        '    lblmens.Text = "No es posible realizar el trámite solicitado para el Nº de Documento ingresado"
        '    txtDocumento.Focus()
        '    DatosValidos = False
        'End If

            If Me.txtTelContacto.Text = "" Or Me.txtApNom.Text = "" Then
                lblmens.Text = "Los campos de Nombre y Teléfono de contacto son obligatorios !"
                Me.txtApNom.Focus()
                DatosValidos = False
        End If
            'Si el subtipo es Entrevista CV, se  valida que tenga el CV cargado
            
                If Me.ddlSubTipoNumero.SelectedValue = 8 Then
                    If Not TieneCV(Me.txtDocumento.Value, ConexionIMPTCE) Then
                        lblmens.Text = "Para sacar un turno para entrevista debe tener su CV cargado con antelación."
                        DatosValidos = False
                    End If
                End If


            hasta = Left(TopeFecha(ConexionGT, Session("Lugar")).ToString, 10)
            desde = Now().Date
            If TurnoTomadoxDocumento(desde, hasta, Me.txtDocumento.Value, lblLugar.Text) Then
                'lblmens.Text = "Ud ya posee un turno asignado dentro del rango de Fechas disponibles!Verifique los datos ingresados."
                TurnoTomadoxDocumentoInfo(desde, hasta, Me.txtDocumento.Value)
                DatosValidos = False
            End If

    End Function
    Function ValidaTurnoTomado(ByVal hoy As Date, ByVal hasta As Date, ByVal doc As String) As Boolean
        Dim con As SqlConnection
        Dim sql As String
        Dim sqlAD As New SqlDataAdapter
        Dim ds As New DataSet

        ValidaTurnoTomado = False



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

        con.Close()


    End Function

    Function ValidaDoc0800(ByVal doc As String) As Boolean
        Dim con As SqlConnection
        Dim sql As String
        Dim sqlAD As New SqlDataAdapter
        Dim ds As New DataSet

        ValidaDoc0800 = False



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
        con.Close()


    

    End Function

    Sub EnviarMailPrueba(ByVal Reserva As String, ByVal Mailto As String, ByVal DirLug As String, ByVal Documento As String) '(ByVal Lugar As Integer, ByVal Sector As Integer, _
        'Como armar el mail
        Dim Correo As New System.Net.Mail.MailMessage
        Correo = New System.Net.Mail.MailMessage()
        'En correo From debe ir el correo del municipio
        Correo.From = New System.Net.Mail.MailAddress("turnos@lomasdezamora.gov.ar")
        Correo.To.Add(Mailto)
        Correo.Subject = "Turno Municipio de Lomas"

        Dim Body As String

        Body = " </span><br /><img src='http://www.lomasdezamora.gov.ar/sacarnumerosviainternet/img/logo.jpg' />" & _
                "<body lang=ES link=blue style='tab-interval:35.4pt'> " & _
                "<div class=Section1> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                "ES(-AR) '>Estimado Contribuyente : <o:p></o:p></span></p> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                "ES(-AR) '><o:p>&nbsp;</o:p></span></p> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                "ES(-AR) '><o:p>&nbsp;</o:p></span></p>" & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                "ES(-AR) '>Usted ha reservado un turno <st1:PersonName " & _
                "ProductID='' w:st='on'></st1:PersonName> " & _
                " a <span class=GramE>las " & Me.ddlturnosDisponibles.SelectedItem.ToString & _
                " el dia " & Me.calFechaTurno.SelectedDate & "</span><o:p></o:p></span></p> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                "ES(-AR) '>Para confirmar el turno debe hacer <a href='http://www.lomasdezamora.gov.ar/sacarnumerosviainternet/ConfirmacionDeTurnos.aspx" & _
                "?ID=" & Reserva & "&Per=" & Documento & _
                "'>Clic Aquí</a><o:p></o:p></span></p> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                "ES(-AR) '><o:p>&nbsp;</o:p></span></p> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                "ES(-AR) '>De lo contrario, por favor haga <a href='http://www.lomasdezamora.gov.ar/sacarnumerosviainternet/AnulacionDeTurnos.aspx" & _
                "?ID=" & Reserva & "&Per=" & Documento & _
                "'>Clic Aquí para anular la reserva</a><o:p></o:p></span></p> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                "ES(-AR) '><o:p>&nbsp;</o:p></span></p> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                "ES(-AR) '>Se le recuerda que usted realizó la reserva para el Documento :" & Me.ddlTipoDoc.SelectedItem.Text.ToString & "-" & Me.txtDocumento.Value.ToString & " a <span class=GramE>las " & Me.ddlturnosDisponibles.SelectedItem.ToString & " </span> " & _
                "y que pasada la media hora sin recibir confirmación de su parte se anulará la misma. <o:p></o:p></span></p>" & _
                " <span lang=ES-AR style='font-family:Verdana;mso-ansi-language:ES-AR'> " & _
                " Deberá acreditarse al ingresar al edificio. <u1:p></u1:p></span><o:p></o:p></p> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language:ES(-AR) '><o:p>&nbsp;</o:p></span></p> " & _
                " <span lang=ES-AR style='font-family:Verdana;mso-ansi-language: ES(-AR) ; font-size: smaller; color: #6633cc;'>" & DirLug & "<o:p></o:p></span> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                "ES(-AR) '>Muchas gracias.<o:p></o:p></span></p> " & _
                "</div> " & _
                "</body> "


        '& "&LUG=" & Lugar & "&SEC=" & Sector & "&TNU=" & TNU & _

        Dim smtp As New System.Net.Mail.SmtpClient
        smtp.Host = "mail.lomasdezamora.gov.ar"

        Correo.Body = Body
        Correo.IsBodyHtml = True
        Correo.Priority = System.Net.Mail.MailPriority.Normal


        'como armar el envio del mail


        'usuario y pass del servidor de correo que tiene que darnos nacho
        '
        'para enviarlo 

        smtp.Send(Correo)

        lblerror.Text = "Mensaje enviado satisfactoriamente"


    End Sub
    Sub EnviarMail(ByVal Reserva As String, ByVal Mailto As String, ByVal DirLug As String, ByVal Documento As String, ByVal CodigoBaja As String) '(ByVal Lugar As Integer, ByVal Sector As Integer, _
        Dim Correo As New System.Net.Mail.MailMessage
        Correo = New System.Net.Mail.MailMessage()
        'En correo From debe ir el correo del municipio
        Correo.From = New System.Net.Mail.MailAddress("turnos2@lomasdezamora.gov.ar")
        Correo.To.Add(Mailto)
        Correo.Subject = "Confirmacion de Turno"

        Dim Body As String

        Body = " </span><br /><img src='http://web2.lomasdezamora.gov.ar/sacarnumerosviainternet/img/logo.jpg' />" & _
                "<body lang=ES link=blue style='tab-interval:35.4pt'> " & _
                "<div class=Section1> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                "ES(-AR) '>Estimado Contribuyente : <o:p></o:p></span></p> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                "ES(-AR) '><o:p>&nbsp;</o:p></span></p> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                "ES(-AR) '><o:p>&nbsp;</o:p></span></p>" & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                "ES(-AR) '>Usted ha reservado un turno <st1:PersonName " & _
                "ProductID='' w:st='on'></st1:PersonName> " & _
                " a <span class=GramE>las " & Me.ddlturnosDisponibles.SelectedItem.ToString & _
                " el dia " & Me.calFechaTurno.SelectedDate & "</span><o:p></o:p></span></p> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                "ES(-AR) '>Para confirmar el turno debe hacer <a href='http://www.lomasdezamora.gov.ar/sacarnumerosviainternet/ConfirmacionDeTurnos.aspx" & _
                "?ID=" & Reserva & "&Per=" & Documento & _
                "'>Clic Aquí</a><o:p></o:p></span></p> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                "ES(-AR) '><o:p>&nbsp;</o:p></span></p> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                "ES(-AR) '>De lo contrario, por favor haga <a href='http://www.lomasdezamora.gov.ar/sacarnumerosviainternet/AnulacionXCodigo.aspx" & _
                "?ID=" & Reserva & "&CodAnul=" & CodigoBaja & "&Per=" & Documento & _
                "'>Clic Aquí para anular la reserva</a><o:p></o:p></span></p> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                "ES(-AR) '><o:p>&nbsp;</o:p></span></p> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                "ES(-AR) '>Se le recuerda que usted realizó la reserva para el Documento :" & Me.ddlTipoDoc.SelectedItem.Text.ToString & "-" & Me.txtDocumento.Value.ToString & " a <span class=GramE>las " & Me.ddlturnosDisponibles.SelectedItem.ToString & " </span> " & _
                "y que pasadas 3 horas sin recibir confirmación de su parte se anulará la misma. <o:p></o:p></span></p>" & _
                " <span lang=ES-AR style='font-family:Verdana;mso-ansi-language:ES-AR'> " & _
                " Deberá acreditarse al ingresar al edificio. <u1:p></u1:p></span><o:p></o:p></p> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language:ES(-AR) '><o:p>&nbsp;</o:p></span></p> " & _
                " <span lang=ES-AR style='font-family:Verdana;mso-ansi-language: ES(-AR) ; font-size: smaller; color: #6633cc;'>" & DirLug & "<o:p></o:p></span> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                "ES(-AR) '>Muchas gracias.<o:p></o:p></span></p> " & _
                "</div> " & _
                "</body> "


        '& "&LUG=" & Lugar & "&SEC=" & Sector & "&TNU=" & TNU & _
        Correo.Body = Body
        Correo.IsBodyHtml = True
        Correo.Priority = System.Net.Mail.MailPriority.Normal


        'como armar el envio del mail
        Dim smtp As New System.Net.Mail.SmtpClient
        smtp.Host = "mail.lomasdezamora.gov.ar"

        'para enviarlo 

        smtp.Send(Correo)

        lblerror.Text = "Mensaje enviado satisfactoriamente"


    End Sub

    Sub EnviarMailConfirmado(ByVal Reserva As String, ByVal Mailto As String, ByVal DirLug As String, ByVal Documento As String, ByVal CodigoBaja As String) '(ByVal Lugar As Integer, ByVal Sector As Integer, _
        Dim Correo As New System.Net.Mail.MailMessage
        Dim LiteralZoo As String
        Correo = New System.Net.Mail.MailMessage()
        'En correo From debe ir el correo del municipio
        Correo.From = New System.Net.Mail.MailAddress("turnos2@lomasdezamora.gov.ar")
        Correo.To.Add(Mailto)
        Correo.Subject = "Confirmacion de Turno"

        Dim Body As String

        If Session("Lugar") = 7 Then
            LiteralZoo = "Concurrir con DNI con domicilio en Lomas de Zamora."
        Else
            LiteralZoo = ""
        End If

        Body = " </span><br />" & _
                "<body lang=ES link=blue style='tab-interval:35.4pt'> " & _
                "<div class=Section1> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                "ES(-AR) '>Estimad(a/o) " & txtApNom.Text.ToString & " : <o:p></o:p></span></p> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                "ES(-AR) '><o:p>&nbsp;</o:p></span></p> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                "ES(-AR) '><o:p>&nbsp;</o:p></span></p>" & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                "ES(-AR) '>Usted ha reservado un turno <st1:PersonName " & _
                "ProductID='' w:st='on'></st1:PersonName> " & _
                " a <span class=GramE>las " & Me.ddlturnosDisponibles.SelectedItem.ToString & _
                " el dia " & Me.calFechaTurno.SelectedDate & "</span><o:p></o:p></span></p> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                " ES(-AR) '>Trámite: " & Me.ddlSubTipoNumero.SelectedItem.ToString & "</span><o:p></o:p></span></p> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                " '>Código de Anulación: " & CodigoBaja & _
                "<o:p>&nbsp;</o:p></span></p>" & _
                " Si desea cancelarlo, por favor haga <a href='http://webextra.lomasdezamora.gov.ar/sacarnumerosviainternet/AnulacionXCodigo.aspx" & _
                "?ID=" & Reserva & "&Per=" & Documento & _
                "'>Clic Aquí para anular la reserva</a><o:p></o:p></span></p> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                "ES(-AR) '><o:p>&nbsp;</o:p></span></p> " & _
                " <span lang=ES-AR style='font-family:Verdana;mso-ansi-language:ES-AR'> " & _
                " Deberá acreditarse al ingresar al edificio. <u1:p></u1:p></span><o:p></o:p></p> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language:ES(-AR) '><o:p>&nbsp;</o:p></span></p> " & _
                " <span lang=ES-AR style='font-family:Verdana;mso-ansi-language:ES-AR'> " & _
                "" & literalZoo & " <u1:p></u1:p></span><o:p></o:p></p> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language:ES(-AR) '><o:p>&nbsp;</o:p></span></p> " & _
                " <span lang=ES-AR style='font-family:Verdana;mso-ansi-language: ES(-AR) ; font-size: smaller; color: #6633cc;'>" & DirLug & "<o:p></o:p></span> " & _
                "<p class=MsoNormal><span lang=ES-AR style='font-family:Verdana;mso-ansi-language: " & _
                "ES(-AR) '>Muchas gracias.<o:p></o:p></span></p> " & _
                "</div> " & _
                "</body> "


        '& "&LUG=" & Lugar & "&SEC=" & Sector & "&TNU=" & TNU & _
        Correo.Body = Body
        Correo.IsBodyHtml = True
        Correo.Priority = System.Net.Mail.MailPriority.Normal

        'adjuntar un archivo al mail(Zoonosis)
        If lblLugar.Text = 65 Then
            Dim at As New Attachment(Server.MapPath("~/img/zoo.jpg"))
            Correo.Attachments.Add(at)
        End If

        'como armar el envio del mail
        Dim smtp As New System.Net.Mail.SmtpClient
        ' smtp.Host = "mail.lomasdezamora.gov.ar"
        smtp.Host = "172.16.0.13"
        'para enviarlo 

        smtp.Send(Correo)

        lblerror.Text = "Mensaje enviado satisfactoriamente"
   
    End Sub

    

  
#End Region

 

End Class
