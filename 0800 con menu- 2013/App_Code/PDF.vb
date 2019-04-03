Imports Microsoft.VisualBasic

Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Text
Imports System.Drawing
Imports System.IO
Imports SqlHelper

Public Class PDF
    Inherits System.Web.UI.Page
    Dim _base As Base = New Base

#Region "PDF"
    Public Sub Borrar()


        Dim direccion As String = Server.MapPath("").ToString
        Dim tomar As Integer = direccion.Length
        'direccion = direccion.Substring(0, (tomar - 15))


        Dim direc As New DirectoryInfo(direccion & "\PDF")
        Dim archivo() As FileInfo
        Dim x As Integer
        archivo = direc.GetFiles
        For x = 0 To archivo.Length - 1
            If archivo(x).Name.ToString.Contains(".pdf") Then

                If BorrarFechaAnterior(archivo(x).Name.ToString) Then
                    archivo(x).Delete()

                End If


            End If
        Next


    End Sub
    Public Function BorrarFechaAnterior(ByVal Cadena As String) As Boolean
        BorrarFechaAnterior = False
        Dim validar As New Validar
        Dim fechadate As String
        Dim x As Integer


        fechadate = ConcatenarFecha(Cadena)



        If validar.ValidarDato(fechadate, Tipodato.Fecha) Then

            Dim tiempoBd As Date = _base.traeerfechaGrabacion()
            'Con esto separas la fechA 

            If Str((DateDiff("s", CDate(fechadate), CDate(tiempoBd)) \ 86400) Mod 365) >= 1 Then
                BorrarFechaAnterior = True
                Exit Function
            End If

            If Str((DateDiff("s", CDate(fechadate), CDate(tiempoBd)) \ 60) Mod 60) >= 1 Then
                BorrarFechaAnterior = True
            End If

        End If
    End Function
    Public Function ConcatenarFecha(cadena As String) As String
        Dim suma As Integer = 1

        If cadena.Substring(1, 1) = "-" Then
            ConcatenarFecha = "0" & cadena.Substring(suma - 1, 1) & "/"
            suma = suma + 2
        Else
            ConcatenarFecha = cadena.Substring(suma - 1, 2) & "/"
            suma = suma + 3
        End If
        If cadena.Substring(suma, 1) = "-" Then
            ConcatenarFecha = ConcatenarFecha & "0" & cadena.Substring(suma - 1, 1) & "/"
            suma = suma + 2
        Else
            ConcatenarFecha = ConcatenarFecha & cadena.Substring(suma - 1, 2) & "/"
            suma = suma + 3
        End If

        ConcatenarFecha = ConcatenarFecha & cadena.Substring(suma - 1, 4) & " "
        suma = suma + 5



        If cadena.Substring(suma, 1) = "-" Then
            ConcatenarFecha = ConcatenarFecha & "0" & cadena.Substring(suma - 1, 1) & ":"
            suma = suma + 2
        Else
            ConcatenarFecha = ConcatenarFecha & cadena.Substring(suma - 1, 2) & ":"
            suma = suma + 3
        End If
        If cadena.Substring(suma, 1) = "-" Then
            ConcatenarFecha = ConcatenarFecha & "0" & cadena.Substring(suma - 1, 1) & ":"
            suma = suma + 2
        Else
            ConcatenarFecha = ConcatenarFecha & cadena.Substring(suma - 1, 2) & ":"
            suma = suma + 3
        End If

        If cadena.Substring(suma, 1) = "-" Then
            ConcatenarFecha = ConcatenarFecha & "0" & cadena.Substring(suma - 1, 1)
        Else
            ConcatenarFecha = ConcatenarFecha & cadena.Substring(suma - 1, 2)
        End If



    End Function

#End Region

End Class
