'{[{
Imports Param_RootNamespace.Views
'}]}

Namespace ViewModels
    Public Class wts.ItemNameViewModel
        Inherits Observable

        '^^
        '{[{
        Private Sub OnsItemSelected(args As ItemClickEventArgs)
            Dim selected = TryCast(args.ClickedItem, SampleImage)
            _imagesGridView.PrepareConnectedAnimation(wts.ItemNameAnimationOpen, selected, "galleryImage")
            ImagesNavigationHelper.AddImageId(ImageGalleryViewSelectedIdKey, selected.ID)
            NavigationService.Navigate(Of wts.ItemNameDetailPage)(selected.ID)
        End Sub
        '}]}
    End Class
End Namespace
