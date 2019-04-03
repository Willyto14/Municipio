Imports Microsoft.VisualBasic

Public Class Mensajes

#Region "Mensajes"
    Public Function Mensajes(Numero As String) As String


        If Left(Numero, 1) = 1 Then
            Mensajes = "Tiempo de session agotado"
        ElseIf Left(Numero, 1) = 2 Then
            Mensajes = "Acceso denegado"
        ElseIf Left(Numero, 1) = 3 Then
            Mensajes = "Turno asiganado con exito , el codigo de anulacion es (" & Numero.Substring(1, Len(Numero) - 1) & ")"
        End If

    End Function

#End Region
End Class
