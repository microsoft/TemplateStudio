Imports Param_ItemNamespace.Models

Namespace Views
    Public NotInheritable Partial Class ImageGalleryViewDetailPage
        Inherits Page

        Public Sub New()
            InitializeComponent()
            ViewModel.SetImage(previewImage)
        End Sub

        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            Await ViewModel.InitializeAsync(TryCast(e.Parameter, SampleImage), e.NavigationMode)
            showFlipView.Begin()
        End Sub

        Protected Overrides Sub OnNavigatingFrom(e As NavigatingCancelEventArgs)
            MyBase.OnNavigatingFrom(e)
            If e.NavigationMode = NavigationMode.Back Then
                previewImage.Visibility = Visibility.Visible
                ViewModel.SetAnimation()
            End If
        End Sub
    End Class
End Namespace
