Imports Param_RootNamespace.Helpers
Imports Param_RootNamespace.Services
Imports Windows.System
Imports Microsoft.Toolkit.Uwp.UI.Animations

Namespace Views
    Public NotInheritable Partial Class ImageGalleryViewDetailPage
        Inherits Page

        Public Sub New()
            InitializeComponent()
        End Sub

        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)

            Await ViewModel.LoadDataAsync()

            Dim selectedImageId = TryCast(e.Parameter, String)
            ViewModel.Initialize(selectedImageId, e.NavigationMode)
        End Sub

        Protected Overrides Sub OnNavigatingFrom(e As NavigatingCancelEventArgs)
            MyBase.OnNavigatingFrom(e)
            If e.NavigationMode = NavigationMode.Back Then
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(ViewModel.SelectedImage)
                ImagesNavigationHelper.RemoveImageId(ImageGalleryViewViewModel.ImageGalleryViewSelectedIdKey)
            End If
        End Sub

        Private Sub OnPageKeyDown(sender As Object, e As KeyRoutedEventArgs)
            If e.Key = VirtualKey.Escape AndAlso NavigationService.CanGoBack Then
                NavigationService.GoBack()
                e.Handled = True
            End If
        End Sub
    End Class
End Namespace
