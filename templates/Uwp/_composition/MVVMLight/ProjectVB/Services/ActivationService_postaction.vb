'{[{
Imports Param_RootNamespace.ViewModels
'}]}
Namespace Services
    Friend Class ActivationService
        Private ReadOnly _defaultNavItem As Type
'{[{

        Private ReadOnly Property NavigationService As NavigationServiceEx
            Get
                Return ViewModelLocator.Current.NavigationService
            End Get
        End Property
'}]}
    End Class
End Namespace