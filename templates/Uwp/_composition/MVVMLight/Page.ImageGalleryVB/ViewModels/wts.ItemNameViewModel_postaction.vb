'{[{
Imports GalaSoft.MvvmLight.Command
'}]}
Imports Param_ItemNamespace.Helpers
Imports Param_ItemNamespace.Services

Namespace ViewModels
    Public Class wts.ItemNameViewModel
        Inherits ViewModelBase

        '{[{
        Public ReadOnly Property NavigationService As NavigationServiceEx
            Get
                Return CommonServiceLocator.ServiceLocator.Current.GetInstance(Of NavigationServiceEx)()
            End Get
        End Property
        '}]}

        '^^
        '{[{
        Private Sub OnsItemSelected(args As ItemClickEventArgs)
            Dim selected = TryCast(args.ClickedItem, SampleImage)
            _imagesGridView.PrepareConnectedAnimation(wts.ItemNameAnimationOpen, selected, "galleryImage")
            ImagesNavigationHelper.AddImageId(wts.ItemNameSelectedIdKey, selected.ID)
            NavigationService.Navigate(GetType(wts.ItemNameDetailViewModel).FullName, selected.ID)
        End Sub
        '}]}
    End Class
End Namespace
