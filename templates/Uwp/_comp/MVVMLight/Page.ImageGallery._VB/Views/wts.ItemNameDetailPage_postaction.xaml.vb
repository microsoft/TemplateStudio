'{[{
Imports Param_RootNamespace.ViewModels
'}]}
Namespace Views
    Public NotInheritable Partial Class wts.ItemNameDetailPage
        Inherits Page
        '{[{

        Public ReadOnly Property NavigationService As NavigationServiceEx
            Get
                Return ViewModelLocator.Current.NavigationService
            End Get
        End Property

        Private ReadOnly Property ViewModel As wts.ItemNameDetailViewModel
            Get
                Return ViewModelLocator.Current.wts.ItemNameDetailViewModel
            End Get
        End Property
        '}]}
    End Class
End Namespace
