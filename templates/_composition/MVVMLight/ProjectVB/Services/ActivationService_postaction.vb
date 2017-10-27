Friend Class ActivationService
    Private ReadOnly _defaultNavItem As Type
'{[{

    Private ReadOnly Property Locator As ViewModels.ViewModelLocator
        Get
            Return TryCast(Application.Current.Resources("Locator"), ViewModels.ViewModelLocator)
        End Get
    End Property

    Private ReadOnly Property NavigationService As NavigationServiceEx
        Get
            Return Locator.NavigationService
        End Get
    End Property
'}]}
End Class
