Imports GalaSoft.MvvmLight.Ioc
Imports Param_RootNamespace.Services
Imports Param_RootNamespace.Views

Namespace ViewModels
    <Windows.UI.Xaml.Data.Bindable>
    Public Class ViewModelLocator
        Private Shared _current As ViewModelLocator

        Public Shared ReadOnly Property Current As ViewModelLocator
            Get
                If _current Is Nothing Then
                    _current = New ViewModelLocator()
                End If
                Return _current
            End Get
        End Property

        Private Sub New()
            SimpleIoc.[Default].Register(Function() New NavigationServiceEx())
        End Sub

        Public ReadOnly Property NavigationService As NavigationServiceEx
          Get
            Return SimpleIoc.[Default].GetInstance(Of NavigationServiceEx)()
          End Get
        End Property

        Private Sub Register(Of VM As Class, V)()
            SimpleIoc.[Default].Register(Of VM)()
            NavigationService.Configure(GetType(VM).FullName, GetType(V))
        End Sub
    End Class
End Namespace
