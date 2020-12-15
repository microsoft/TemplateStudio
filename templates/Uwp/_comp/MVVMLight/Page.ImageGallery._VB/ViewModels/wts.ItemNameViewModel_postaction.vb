'{[{
Imports GalaSoft.MvvmLight.Command
Imports Microsoft.Toolkit.Uwp.UI.Animations
'}]}
Imports Param_RootNamespace.Helpers
Imports Param_RootNamespace.Services

Namespace ViewModels
    Public Class wts.ItemNameViewModel
        Inherits ViewModelBase

        '{[{
        Public ReadOnly Property NavigationService As NavigationServiceEx
            Get
                Return ViewModelLocator.Current.NavigationService
            End Get
        End Property
        '}]}

        '^^
        '{[{
        Private Sub OnItemSelected(args As ItemClickEventArgs)
            Dim selected = TryCast(args.ClickedItem, SampleImage)
            ImagesNavigationHelper.AddImageId(wts.ItemNameSelectedIdKey, selected.ID)
            NavigationService.Frame.SetListDataItemForNextConnectedAnimation(selected)
            NavigationService.Navigate(GetType(wts.ItemNameDetailViewModel).FullName, selected.ID)
        End Sub
        '}]}
    End Class
End Namespace
