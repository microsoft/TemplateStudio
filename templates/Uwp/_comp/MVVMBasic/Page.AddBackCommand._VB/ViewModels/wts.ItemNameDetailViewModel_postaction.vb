'{[{
Imports Param_RootNamespace.Services
'}]}
Namespace ViewModels
    Public Class wts.ItemNameDetailViewModel
        Inherits Observable

        '^^
        '{[{

        Public ReadOnly Property GoBackCommand As ICommand = new RelayCommand(AddressOf OnGoBack)
        '}]}

        Public Sub New()
        End Sub

        '^^
        '{[{
        Private Sub OnGoBack()
            If NavigationService.CanGoBack Then
                NavigationService.GoBack()
            End If
        End Sub
        '}]}
    End Class
End Namespace
