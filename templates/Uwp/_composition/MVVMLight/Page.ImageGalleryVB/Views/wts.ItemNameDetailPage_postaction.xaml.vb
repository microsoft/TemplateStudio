'{[{
Imports Param_ItemNamespace.ViewModels
'}]}
Namespace Views
    Public NotInheritable Partial Class wts.ItemNameDetailPage
        Inherits Page
        '{[{

        Public ReadOnly Property NavigationService As NavigationServiceEx
            Get
                Return CommonServiceLocator.ServiceLocator.Current.GetInstance(Of NavigationServiceEx)()
            End Get
        End Property

        Private ReadOnly Property ViewModel As wts.ItemNameDetailViewModel
            Get
                Return TryCast(DataContext, wts.ItemNameDetailViewModel)
            End Get
        End Property
        '}]}
    End Class
End Namespace
