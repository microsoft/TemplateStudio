Imports Param_ItemNamespace.Models
Imports Param_ItemNamespace.Services
Imports Windows.System

Namespace Views
    Public NotInheritable Partial Class ImageGalleryViewDetailPage
        Inherits Page

        Public Sub New()
            InitializeComponent()
            ViewModel.SetImage(previewImage)
        End Sub

        Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            ViewModel.Initialize(TryCast(e.Parameter, String), e.NavigationMode)
            showFlipView.Begin()
        End Sub

        Protected Overrides Sub OnNavigatingFrom(e As NavigatingCancelEventArgs)
            MyBase.OnNavigatingFrom(e)
            If e.NavigationMode = NavigationMode.Back Then
                previewImage.Visibility = Visibility.Visible
                ViewModel.SetAnimation()
            End If
        End Sub

        Private Sub OnShowFlipViewCompleted(sender As Object, e As Object)
            flipView.Focus(FocusState.Programmatic)
        End Sub

        Private Sub OnPageKeyDown(sender As Object, e As KeyRoutedEventArgs)
            If e.Key = VirtualKey.Escape AndAlso NavigationService.CanGoBack Then
                NavigationService.GoBack()
                e.Handled = True
            End If
        End Sub
    End Class
End Namespace
