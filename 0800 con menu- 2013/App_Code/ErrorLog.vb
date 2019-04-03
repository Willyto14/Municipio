Imports Microsoft.VisualBasic

Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Text
Imports System.Drawing
Imports System.IO
Imports SqlHelper

Public Class ErrorLog
    Inherits Base

#Region "Escrivir y Crear"
    Public Sub LogError(ex As Exception)


        Dim message As String = String.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"))
        message += Environment.NewLine
        message += "-----------------------------------------------------------"
        message += Environment.NewLine
        message += String.Format("Message: {0}", ex.Message)
        message += Environment.NewLine
        message += String.Format("StackTrace: {0}", ex.StackTrace)
        message += Environment.NewLine
        message += String.Format("Source: {0}", ex.Source)
        message += Environment.NewLine
        message += String.Format("TargetSite: {0}", ex.TargetSite.ToString())
        message += Environment.NewLine
        message += "-----------------------------------------------------------"
        message += Environment.NewLine


        Dim path As String = Server.MapPath("~/ErrorLog/" & Nombredetxt())
        Using writer As New StreamWriter(path, True)
            writer.WriteLine(message)
            writer.Close()
        End Using

    End Sub

    Public Function Nombredetxt() As String
        Dim FechaBd As Date = traeerfechaGrabacion()
        Nombredetxt += FechaBd.Day & "-" & FechaBd.Month & "-" & FechaBd.Year & ".txt"
        Crear(Nombredetxt)
    End Function

    Public Sub Crear(Nombredetxt As String)


        Dim direccion As String = Server.MapPath("").ToString
        Dim tomar As Integer = direccion.Length



        Dim direc As New DirectoryInfo(direccion & "\ErrorLog")
        Dim archivo() As FileInfo
        Dim x As Integer
        archivo = direc.GetFiles

        For x = 0 To archivo.Length - 1
            If archivo(x).Name.ToString.Contains(".txt") Then
                'Comprobar si no existe
                If Nombredetxt.ToString = archivo(x).Name.ToString Then
                    Exit Sub
                End If
            End If
        Next

        ':::Ruta donde crearemos nuestro archivo txt
        Dim ruta As String = direccion & "\ErrorLog\"
        ':::Nombre del archivo
        Dim archivos As String = Nombredetxt

        Dim fs As FileStream
        fs = File.Create(ruta & archivos)
        fs.Close()
    End Sub
#End Region

#Region "Borrar"
    Public Sub Borrar()


        Dim direccion As String = Server.MapPath("").ToString
        Dim tomar As Integer = direccion.Length



        Dim direc As New DirectoryInfo(direccion & "\ErrorLog")
        Dim archivo() As FileInfo
        Dim x As Integer
        archivo = direc.GetFiles
        For x = 0 To archivo.Length - 1
            If archivo(x).Name.ToString.Contains(".txt") Then

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

            Dim tiempoBd As Date = traeerfechaGrabacion()
            'Con esto separas la fechA 

            If Str((DateDiff("s", CDate(fechadate), CDate(tiempoBd)) \ 86400) Mod 365) >= 7 Then
                BorrarFechaAnterior = True
                Exit Function
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


    End Function
#End Region

End Class
