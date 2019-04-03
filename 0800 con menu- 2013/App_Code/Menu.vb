Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports SqlHelper

Public Class Menu
#Region "VARIABLES"
    Dim cnString As String = ConfigurationManager.ConnectionStrings("myConexion").ConnectionString
    Dim _Id As Integer
    Dim _Nivel As Integer
    Dim _Descripcion As String
    Dim _Url As String
    Dim _Permiso As String
    Dim _Imagen As String
    Dim _Target As String
#End Region

#Region "PROPIEDADES"
    Public Property Id() As Integer
        Get
            Return _Id
        End Get
        Set(ByVal value As Integer)
            _Id = value
        End Set
    End Property
    Public Property Nivel() As Integer
        Get
            Return _Nivel
        End Get
        Set(ByVal value As Integer)
            _Nivel = value
        End Set
    End Property
    Public Property Descripcion() As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
        End Set
    End Property
    Public Property Url() As String
        Get
            Return _Url
        End Get
        Set(ByVal value As String)
            _Url = value
        End Set
    End Property
    Public Property Imagen() As String
        Get
            Return _Imagen
        End Get
        Set(ByVal value As String)
            _Imagen = value
        End Set
    End Property

    Public Property Permiso() As Boolean
        Get
            Return _Permiso
        End Get
        Set(ByVal value As Boolean)
            _Permiso = value
        End Set
    End Property

    Public Property Target() As String
        Get
            Return _Target
        End Get
        Set(ByVal value As String)
            _Target = value
        End Set
    End Property
#End Region

#Region "CONSTRUCTORES"
    Public Sub New()

    End Sub

    Public Sub New(ByVal dr As DataRow)
        SetMe(dr)
    End Sub
#End Region

#Region "METODOS"
    Private Sub SetMe(ByVal dr As DataRow)
        Me.Id = dr(0)
        Me.Nivel = dr(1)
        Me.Descripcion = dr(2)
        Me.Url = dr(3)
        Me.Permiso = dr(4)
        Me.Imagen = dr(5)
        Me.Target = dr(6)
    End Sub

    Public Function ObtenerPermisosMenu(ByVal IdUsuario As Integer) As ArrayList
        Dim ds As New DataSet
        Dim objMenu As Menu
        Dim arrMenu As New ArrayList
        Try
            ds = SqlHelper.ExecuteDataset(cnString, CommandType.StoredProcedure, "SP_PermisosMenu_SEL", New SqlClient.SqlParameter("@id_usuario", IdUsuario))
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    For Each dr As DataRow In ds.Tables(0).Rows
                        objMenu = New Menu(dr)
                        arrMenu.Add(objMenu)
                    Next
                End If
            End If
        Catch ex As Exception
            Throw New Exception("Error al obtener los permisos del usuario")
        End Try
        Return arrMenu
    End Function
#End Region
End Class
