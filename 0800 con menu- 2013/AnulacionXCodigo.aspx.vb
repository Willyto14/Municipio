Imports System.Data
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data.SqlClient

Partial Class AnulacionXCodigo
    Inherits Base

    '*****Variables para Usuario y contraseña encriptada

    Dim _errorLog As ErrorLog = New ErrorLog()


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblDocumento.Visible = False
        lblFecha.Visible = False
        lblNombre.Visible = False
        lblTipoDoc.Visible = False
        Label1.Visible = False
        label3.Visible = False
        lblTurno.Visible = False
        lblTipoTramite.Visible = False


    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Response.Redirect("principal.aspx")
    End Sub

    Protected Sub cmdAnulaTurno_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAnulaTurno.Click
        Dim ID As String
        Dim DNI As String

        If Me.txtCodigoAnulacion.Text = "" Then
            Dim mensajeError As New Mensajes()
            lblMensajeError.Text = "Ingrese un código de Anulación por favor"
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)

            Exit Sub
        Else
            ID = Left(Me.txtCodigoAnulacion.Text, 3)
            DNI = Mid(Me.txtCodigoAnulacion.Text, 4, (Len(Me.txtCodigoAnulacion.Text) - 3))
        End If
        If BuscoTurnoParaBaja(ID, DNI) Then
            TraigoDatosTurno()
            BajadeTurno(DNI)
            Me.txtCodigoAnulacion.Text = ""
        End If
    End Sub

    Sub BajadeTurno(ByVal dni As String)
        Dim con As System.Data.SqlClient.SqlConnection
        Dim dred As System.Data.SqlClient.SqlDataReader

        Try

            If lblSobreTurno.Text <> True Then



                dred = SqlHelper.ExecuteReader(ConexionGT, CommandType.StoredProcedure, "SP_ReservaDeTurnoNUEVOWil", _
                         New SqlParameter("@stnu", DBNull.Value), _
                         New SqlParameter("@tdni", DBNull.Value), _
                         New SqlParameter("@doc", DBNull.Value), _
                         New SqlParameter("@mail", DBNull.Value), _
                         New SqlParameter("@AtenV", DBNull.Value), _
                         New SqlParameter("@ApNom", DBNull.Value), _
                         New SqlParameter("@TelC", DBNull.Value), _
                         New SqlParameter("@conf", 0), _
                         New SqlParameter("@ID", Session("ID").ToString), _
                         New SqlParameter("@Documento", dni.ToString))

            Else
                dred = SqlHelper.ExecuteReader(ConexionGT, CommandType.StoredProcedure, "SP_ReservaDeTurnoNUEVOWilSobreTurno", _
                       New SqlParameter("@ID", Session("ID").ToString), _
                       New SqlParameter("@Documento", dni.ToString))

            End If
            dred.Read()

            If dred.Item(0) = 0 Then
                Dim mensajeError As New Mensajes()
                lblMensajeError.Text = "Lamentablemente su turno no ha podido ser anulado!"
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
            End If
            dred.Close()

            con = New System.Data.SqlClient.SqlConnection()
            con.ConnectionString = ConexionGT()


        Catch er As Exception
            'MessageBox(Err.Description)
        End Try

    End Sub

    Function BuscoTurnoParaBaja(ByVal id As String, ByVal dni As String) As Boolean
        Dim con As System.Data.SqlClient.SqlConnection
        Dim reader As System.Data.SqlClient.SqlDataReader
        Dim sql As System.Data.SqlClient.SqlCommand
        Dim sSql As String

        sSql = "  Select TUR_Identificacion, TUR_ID ,TUR_SobreTurno from Turnos where "
        sSql = sSql & " left(TUR_ID, 3) = '" & id & "'"
        sSql = sSql & " and TUR_Documento = '" & dni & "'"
        sSql = sSql & " and TUR_BajaLogica is NULL"
        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = ConexionGT()

        Try
            con.Open()

            sql = New System.Data.SqlClient.SqlCommand
            sql.CommandText = sSql


            sql.Connection = con

            reader = sql.ExecuteReader()
            reader.Read.ToString()

            If reader.HasRows Then
                BuscoTurnoParaBaja = True
                Session("Unique") = reader.Item(0)
                Session("ID") = reader.Item(1)
            Else
                BuscoTurnoParaBaja = False
                Dim mensajeError As New Mensajes()
                lblMensajeError.Text = "El código de anulación es inexistente"
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)

            End If

            reader.Close()
            con.Close()
        Catch er As Exception
            'MessageBox(Err.Description)
            EstadoDeConexion(con)
            EstadoDeConexion(reader)
        End Try

    End Function

    Sub TraigoDatosTurno()
        Dim con As System.Data.SqlClient.SqlConnection
        Dim reader As System.Data.SqlClient.SqlDataReader
        Dim sql As System.Data.SqlClient.SqlCommand
        Dim sSql As String

        sSql = " select STNU_Descripcion, TUR_FechaTurno, "
        sSql = sSql & " TUR_ApNomC, TUR_TipoDocumento, TUR_Documento "
        sSql = sSql & "  , Case  When TUR_SobreTurno = 'True' then 1 "
        sSql = sSql & " Else 0 End  as TUR_SobreTurno"
        sSql = sSql & " From Turnos t inner join SubTipos_Numeros s "
        sSql = sSql & " on t.STNU_Codigo = s.STNU_Codigo "
        sSql = sSql & " where TUR_Identificacion = " & Session("Unique")

        con = New System.Data.SqlClient.SqlConnection()
        con.ConnectionString = ConexionGT()

        Try
            con.Open()

            sql = New System.Data.SqlClient.SqlCommand
            sql.CommandText = sSql

            sql.Connection = con

            reader = sql.ExecuteReader()
            reader.Read.ToString()

            If reader.HasRows Then
                lblDocumento.Visible = True
                lblFecha.Visible = True
                lblNombre.Visible = True
                lblTipoDoc.Visible = True
                Label1.Visible = True
                label3.Visible = True
                lblTurno.Visible = True
                lblTipoTramite.Visible = True
                Me.lblTipoTramite.Text = reader.Item(0)
                Me.lblFecha.Text = reader.Item(1) & " horas"
                Me.lblNombre.Text = reader.Item(2)
                Me.lblTipoDoc.Text = reader.Item(3)
                Me.lblDocumento.Text = reader.Item(4)
                Me.lblSobreTurno.Text = reader.Item(5).ToString
            End If


            reader.Close()
            con.Close()
        Catch er As Exception
            'MessageBox(Err.Description)
            EstadoDeConexion(con)
            EstadoDeConexion(reader)
        End Try

    End Sub



End Class

  



