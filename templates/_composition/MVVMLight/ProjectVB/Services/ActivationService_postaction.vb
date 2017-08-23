Friend Class ActivationService
    Private ReadOnly _defaultNavItem As Type
'{[{

    Private ReadOnly Property NavigationService() As NavigationServiceEx
        Get
            Return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of NavigationServiceEx)()
        End Get
    End Property

'}]}
End Class
