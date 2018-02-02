'{[{
Imports Param_ItemNamespace.ViewModels
'}]}
Namespace Views
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page
'{[{

        Private ReadOnly Property ViewModel As wts.ItemNameViewModel
            Get
                Return TryCast(DataContext, wts.ItemNameViewModel)
            End Get
        End Property
'}]}
    End Class
End Namespace
