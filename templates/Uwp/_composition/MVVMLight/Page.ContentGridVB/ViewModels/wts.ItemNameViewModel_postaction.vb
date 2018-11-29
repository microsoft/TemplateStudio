'{[{
Imports GalaSoft.MvvmLight.Command
'}]}

Namespace ViewModels
    Public Class wts.ItemNameViewModel
'{[{
        Public ReadOnly Property NavigationService As NavigationServiceEx
            Get
                Return CommonServiceLocator.ServiceLocator.Current.GetInstance(Of NavigationServiceEx)()
            End Get
        End Property
'}]}

        Private Sub OnsItemSelected(args As ItemClickEventArgs)
            If item IsNot Nothing Then
'^^
'{[{
                NavigationService.Navigate(GetType(wts.ItemNameDetailViewModel).FullName, item.OrderId)
'}]}
            End If
        End Sub
    End Class
End Namespace
