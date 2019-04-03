Imports System
Imports System.IO
Imports System.Security
Imports System.Security.Cryptography
Imports System.Text
Public Class Encriptacion
#Region "Contraseñas Hash"
    'Con esta función que tenemos aquí abajo, lo que haces es transformar la cadena en Bytes.

    Shared Function ConvertirCadenaEnMatrizDeTipoByte(ByVal Cadena As [String]) As [Byte]()

        Return (New UnicodeEncoding).GetBytes(Cadena)

        'Retornamos la cadena convertida.

    End Function


    'En esta función la cadena la convertiremos en Bytes con la función anterior, lo transformaremos en un valor Hash, encriptado con el algoritmo SHA512 y más tarde lo convertiremos en una cadena.


    Public Function EncriptarHash(ByVal ValorAEncriptar As String) As String

        Dim ValorToHash As [Byte]() = ConvertirCadenaEnMatrizDeTipoByte(ValorAEncriptar)

        'Declaramos la variable de tipo Byte y llamamos a ConvertirCadenaEnMatrizDeTipoByte y convertimos la cadena a encriptar en typo Byte

        Dim ValorHash As Byte() = CType(CryptoConfig.CreateFromName("SHA512"), HashAlgorithm).ComputeHash(ValorToHash)

        'Encriptamos variable Byte en una cadena de tipo Byte, también.

        Return Convert.ToBase64String(ValorHash)

        'BitConverter.ToString(ValorHash)

        'Retornamos la variable de tipo Byte en String.

    End Function


    'Para comparar dos valores lo primero es enviar como parametros una cadena sin encriptar y otra cadena encriptada, con el mismo tipo de encriptación, y haciendo uso del mismo algoritmo, en nuestro caso usaremos Hash con el algoritmo SHA512. Luego encriptamos el valor si encriptar y comparamos los dos valores, retornando True si son iguales y False si son diferentes, esto lo podemos hacer con una simple sentencia IF.

    Public Function Comparar(ByVal ValorSinEncriptar As String, ByVal ValorEncriptado As String) As Boolean

        If EncriptarHash(ValorSinEncriptar) = ValorEncriptado Then

            Return True

        Else

            Return False

        End If

    End Function

#End Region



#Region "Encriptar/Desencriptar"

    Const StrClave As String = "EjemploDeCodigo"

    Public Function EncriptarTexto(ByVal CadenaAEncriptar As String, Optional ByVal Clave As String = StrClave, Optional ByVal MantenerMayusculas As Boolean = True)

        Dim i As Integer, C As Integer

        Dim CadenaEncriptada As String = ""

        If MantenerMayusculas = False Then

            Clave = Clave.ToUpper()

            CadenaAEncriptar = CadenaAEncriptar.ToUpper()

        End If

        If Len(Clave) Then

            For i = 1 To Len(CadenaAEncriptar)

                C = Asc(Mid$(CadenaAEncriptar, i, 1))

                C = C + Asc(Mid$(Clave, (i Mod Len(Clave)) + 1, 1))

                CadenaEncriptada = CadenaEncriptada & Chr(C And &HFF)

            Next i

        Else

            CadenaEncriptada = CadenaAEncriptar

        End If

        Return CadenaEncriptada

    End Function



    Public Function DesencriptarTexto(ByVal CadenaADesencriptar As String, Optional ByVal Clave As String = StrClave, Optional ByVal MantenerMayusculas As Boolean = True)

        Dim i As Integer, C As Integer

        Dim CadenaDesencriptada As String = ""

        If MantenerMayusculas = False Then

            Clave = Clave.ToUpper

        End If

        If Len(Clave) Then

            For i = 1 To Len(CadenaADesencriptar)

                C = Asc(Mid$(CadenaADesencriptar, i, 1))

                C = C - Asc(Mid$(Clave, (i Mod Len(Clave)) + 1, 1))

                CadenaDesencriptada += Chr(C And &HFF)

            Next i

        Else

            CadenaDesencriptada = CadenaADesencriptar

        End If

        Return CadenaDesencriptada

    End Function

#End Region

End Class

