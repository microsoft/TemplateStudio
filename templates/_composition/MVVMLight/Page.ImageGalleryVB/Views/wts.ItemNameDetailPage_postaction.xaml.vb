'{[{
Imports Param_ItemNamespace.ViewModels
'}]}
Namespace Views
    Public NotInheritable Partial Class wts.ItemNameDetailPage
        Inherits Page
        '{[{

        Private ReadOnly Property ViewModel As wts.ItemNameDetailViewModel
            Get
                Return TryCast(DataContext, wts.ItemNameDetailViewModel)
            End Get
        End Property
        '}]}
    End Class
End Namespace
