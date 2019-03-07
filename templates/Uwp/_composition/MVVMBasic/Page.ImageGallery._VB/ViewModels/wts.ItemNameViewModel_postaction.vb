'{[{
Imports Param_RootNamespace.Views
Imports Microsoft.Toolkit.Uwp.UI.Animations
'}]}

Namespace ViewModels
    Public Class wts.ItemNameViewModel
        Inherits Observable

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
