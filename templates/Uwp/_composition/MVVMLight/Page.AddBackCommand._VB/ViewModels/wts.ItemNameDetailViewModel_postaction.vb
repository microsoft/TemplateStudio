'{[{
Imports Param_RootNamespace.Services
Imports GalaSoft.MvvmLight.Command
'}]}
Namespace ViewModels
    Public Class wts.ItemNameDetailViewModel
        Inherits ViewModelBase

        '^^
        '{[{

        Private ReadOnly Property NavigationService As NavigationServiceEx
            Get
                Return ViewModelLocator.Current.NavigationService
            End Get
        End Property

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
