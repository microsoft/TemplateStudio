Imports GalaSoft.MvvmLight.Ioc
Imports Microsoft.Practices.ServiceLocation
Imports Param_RootNamespace.Services
Imports Param_RootNamespace.Views

Namespace ViewModels
    Public Class ViewModelLocator
        Private _navigationService As New NavigationServiceEx()

        Public Sub New()
            ServiceLocator.SetLocatorProvider(Function() SimpleIoc.[Default])

            SimpleIoc.[Default].Register(Function() _navigationService)
        End Sub

        Public Sub Register(Of VM As Class, V)()
            SimpleIoc.[Default].Register(Of VM)()
            _navigationService.Configure(GetType(VM).FullName, GetType(V))
        End Sub
    End Class
End Namespace
