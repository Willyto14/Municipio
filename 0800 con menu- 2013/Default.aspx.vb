Imports System.Web.UI.HtmlControls.HtmlInputText
Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net
Imports System.Net.Mail
Partial Class _Default
    Inherits Base

#Region "Botones"
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim Mensaje As Integer = CInt(Request.Params("MensajeError"))
        If Mensaje = 1 Then
            Dim mensajeError As New Mensajes()
            lblMensajeError.Text = mensajeError.Mensajes(Mensaje)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End If
    End Sub
    Protected Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        Try
            Dim en As New Encriptacion
            Dim objLogin As New Login
            Dim ds As New DataSet
            Dim sSql As String


            If ValidarLoguin("Entrar") Then



                sSql = "Select  CO_DNI ,CO_Admin,CO_SobreTurno"
                sSql = sSql & " FROM O8OO"
                sSql = sSql & " Where   CO_DNI ='" & txtUsuario.Text & "'"
                sSql = sSql & " and CO_Pass = '" & en.EncriptarTexto(txtContraseña.Text) & "'"
                sSql = sSql & " and  CO_BajaLogica is NULL"


                ds = EjecutarDataSet(sSql, ConexionGT)

                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        For Each dr As DataRow In ds.Tables(0).Rows
                            Session.Add("Dni", txtUsuario.Text)
                            If dr(1).ToString = "" Then
                                Session.Add("Administrador", False)
                            Else
                                Session.Add("Administrador", dr(1).ToString)
                            End If
                            If dr(2).ToString = "" Or dr(2).ToString = False Then
                                Session.Add("SobreTurno", False)
                            Else
                                Session.Add("SobreTurno", True)
                            End If
                            Session.Add("Password", en.EncriptarTexto(txtContraseña.Text))
                            Session("Password").ToString()

                        Next
                        Response.Redirect("~/Principal.aspx")
                    Else
                        lblMensajeError.Visible = True
                        lblMensajeError.Text = "Usuario o contraseña incorrectos"
                        Exit Sub
                    End If

                End If

                End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "keyDetalleError", "mostrarError();", True)
        End Try
    End Sub
#End Region

#Region "Metodos"
    Public Function ValidarLoguin(Validar As String) As Boolean
        ValidarLoguin = True
        lblMensajeError.Visible = False
        Dim _validar As New Validar
        If Validar = "Entrar" Then
            If txtUsuario.Text = "" Then
                ValidarLoguin = False
                lblMensajeError.Visible = True
                lblMensajeError.Text = "Ingrese Usuario"
                Exit Function
            End If
            If txtContraseña.Text = "" Then
                ValidarLoguin = False
                lblMensajeError.Visible = True
                lblMensajeError.Text = "Ingrese Contraseña"
                Exit Function
            End If
            If _validar.ValidarDato(txtUsuario.Text, Tipodato.Entero) = False Then
                ValidarLoguin = False
                lblMensajeError.Visible = True
                lblMensajeError.Text = "El campo Usuario solo admite numeros"
                Exit Function
            End If

        End If


    End Function
#End Region


End Class