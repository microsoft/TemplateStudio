'{[{
Imports Param_RootNamespace.Views
Imports Microsoft.Toolkit.Uwp.UI.Animations
Imports Microsoft.Toolkit.Mvvm.Input
'}]}

Namespace ViewModels
    Public Class wts.ItemNameViewModel
        Inherits ObservableObject

        '^^
        '{[{
        Private Sub OnsItemSelected(args As ItemClickEventArgs)
            Dim selected = TryCast(args.ClickedItem, SampleImage)
            ImagesNavigationHelper.AddImageId(wts.ItemNameSelectedIdKey, selected.ID)
            NavigationService.Frame.SetListDataItemForNextConnectedAnimation(selected)
            NavigationService.Navigate(Of wts.ItemNameDetailPage)(selected.ID)
        End Sub
        '}]}
    End Class
End Namespace
