Imports Microsoft.VisualBasic
Public Enum Tipodato
    CadenaDeCaracteres
    Entero
    Flotante
    Fecha
    Money
    Imagen
End Enum
Public Class Validar
    Inherits System.Web.UI.Page

#Region "Metodos y Funciones de validacion"

    Public Function ValidarDato(ByVal CampoAValidar As String, ByVal TipoDeDatoAValidar As Tipodato) As Boolean
        ValidarDato = True
        Try
            If TipoDeDatoAValidar = Tipodato.CadenaDeCaracteres Then
                CampoAValidar = CStr(CampoAValidar)
                Exit Function
            ElseIf TipoDeDatoAValidar = Tipodato.Entero Then
                CampoAValidar = CInt(CampoAValidar)
                Exit Function
            ElseIf TipoDeDatoAValidar = Tipodato.Flotante Then
                CampoAValidar = CDbl(CampoAValidar)
                Exit Function
            ElseIf TipoDeDatoAValidar = Tipodato.Fecha Then
                If CampoAValidar.ToString.Count > 9 Then
                    CampoAValidar = CDate(CampoAValidar)
                Else
                    ValidarDato = False
                End If

                Exit Function
            ElseIf TipoDeDatoAValidar = Tipodato.Money Then
                CampoAValidar = CDbl(CampoAValidar)
                Exit Function
            ElseIf TipoDeDatoAValidar = Tipodato.Imagen Then
                If Right(CampoAValidar, 4) <> ".jpg" And Right(CampoAValidar, 4) <> ".bmp" And Right(CampoAValidar, 4) <> ".png" Then
                    ValidarDato = False
                End If
            End If
        Catch ex As Exception
            ValidarDato = False

        End Try
        Return ValidarDato
    End Function
    Public Function ValidarTextboxCampoVacio(txt As TextBox, lbl As Label, ValidarDatos As Boolean) As Boolean
        ValidarTextboxCampoVacio = True
        If txt.Text = vbNullString Then
            txt.BorderColor = System.Drawing.Color.Red
            If ValidarDatos = True Then
                lbl.Text = "El Campo No Puede estar vacio"
                lbl.Visible = True
                txt.Focus()
                ValidarTextboxCampoVacio = False
            End If

        Else
            txt.BorderColor = System.Drawing.Color.Empty

        End If
        If ValidarDatos = False Then
            ValidarTextboxCampoVacio = False
        End If
    End Function
    Public Function ValidarTextboxFecha(txt As TextBox, lbl As Label, foco As Boolean) As Boolean
        ValidarTextboxFecha = True
        If ValidarDato(txt.Text, Tipodato.Fecha) = False Then
            txt.BorderColor = System.Drawing.Color.Red

            lbl.Visible = True
            ValidarTextboxFecha = False
            If foco = False Then
                lbl.Text = "Fecha Invalida"
                txt.Focus()
            End If
        Else
            txt.BorderColor = System.Drawing.Color.Empty

        End If

    End Function
    Public Function ValidarFileUploadVacio(fu As FileUpload, lbl As Label) As Boolean
        ValidarFileUploadVacio = True
        If fu.HasFile = False Then
            ValidarFileUploadVacio = False
            lbl.Text = "Debe Seleccionar Imagen"
        End If



    End Function
    Public Function ValidarFileUploadImagen(fu As FileUpload, lbl As Label, foco As Boolean) As Boolean
        ValidarFileUploadImagen = True
        If ValidarDato(fu.FileName, Tipodato.Imagen) = False Then
            ValidarFileUploadImagen = False
            If foco = False Then
                lbl.Text = "Debe Seleccionar Imagen .jpg , .bmp o .png"
            End If

            lbl.Visible = True
        End If



    End Function
    Public Sub BlanqueoTexbox(txt As TextBox, color As Boolean)
        txt.Text = ""
        If color Then
            txt.BorderColor = System.Drawing.Color.Empty
        End If

    End Sub
    Public Sub BlanqueoDropDownList(Ddl As DropDownList, color As Boolean)
        Ddl.Items.Clear()
        If color Then
            Ddl.BorderColor = System.Drawing.Color.Empty
        End If

    End Sub
    Public Function ValidarDropDownListVacio(Ddl As DropDownList, lbl As Label) As Boolean
        ValidarDropDownListVacio = True
        If Ddl.SelectedItem.Text = "Seleccionar..." Then
            ValidarDropDownListVacio = False
            lbl.Text = "Debe seleccionar un item"
            lbl.Visible = True
            Ddl.BorderColor = System.Drawing.Color.Red
        Else
            Ddl.BorderColor = System.Drawing.Color.Empty

        End If



    End Function

  
#End Region

End Class
