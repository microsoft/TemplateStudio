Imports GalaSoft.MvvmLight.Ioc
Imports Microsoft.Practices.ServiceLocation
Imports Param_RootNamespace.Services
Imports Param_RootNamespace.Views

Namespace ViewModels
    Public Class ViewModelLocator
        Public Sub New()
            ServiceLocator.SetLocatorProvider(Function() SimpleIoc.[Default])

            SimpleIoc.[Default].Register(Function() New NavigationServiceEx())
        End Sub

        Public ReadOnly Property NavigationService As NavigationServiceEx
          Get
            Return ServiceLocator.Current.GetInstance(Of NavigationServiceEx)()
          End Get
        End Property

        Public Sub Register(Of VM As Class, V)()
            SimpleIoc.[Default].Register(Of VM)()
            _navigationService.Configure(GetType(VM).FullName, GetType(V))
        End Sub
    End Class
End Namespace
