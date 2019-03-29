'{[{
Imports Param_RootNamespace.ViewModels
'}]}
Namespace Views
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page
'{[{

        Private ReadOnly Property ViewModel As wts.ItemNameViewModel
            Get
                Return ViewModelLocator.Current.wts.ItemNameViewModel
            End Get
        End Property
'}]}
    End Class
End Namespace
