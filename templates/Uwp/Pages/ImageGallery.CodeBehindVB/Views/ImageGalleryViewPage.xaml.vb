Imports Windows.Storage
Imports Windows.UI.Xaml.Media.Animation

Imports Param_ItemNamespace.Helpers
Imports Param_ItemNamespace.Models
Imports Param_ItemNamespace.Services

Namespace Views
    Public NotInheritable Partial Class ImageGalleryViewPage
        Inherits Page
        Implements System.ComponentModel.INotifyPropertyChanged

        Public Const ImageGalleryViewSelectedIdKey As String = "ImageGalleryViewSelectedIdKey"
        Public Const ImageGalleryViewAnimationOpen As String = "ImageGalleryView_AnimationOpen"
        Public Const ImageGalleryViewAnimationClose As String = "ImageGalleryView_AnimationClose"

        Private _source As ObservableCollection(Of SampleImage)

        Public Property Source As ObservableCollection(Of SampleImage)
            Get
                Return _source
            End Get
            Set
                [Param_Setter](_source, value)
            End Set
        End Property

        Public Sub New()
            ' TODO WTS: Replace this with your actual data
            Source = SampleDataService.GetGallerySampleData()
            InitializeComponent()
        End Sub

        Private Sub ImagesGridView_ItemClick(sender As Object, e As ItemClickEventArgs)
            Dim selected = TryCast(e.ClickedItem, SampleImage)
            ImagesGridView.PrepareConnectedAnimation(ImageGalleryViewAnimationOpen, selected, "galleryImage")
            NavigationService.Navigate(Of ImageGalleryViewDetailPage)(e.ClickedItem)
        End Sub

        Private Async Sub ImagesGridView_Loaded(sender As Object, e As RoutedEventArgs)
            Dim selectedImageId = Await ApplicationData.Current.LocalSettings.ReadAsync(Of String)(ImageGalleryViewSelectedIdKey)
            If Not String.IsNullOrEmpty(selectedImageId) Then
                Dim animation = ConnectedAnimationService.GetForCurrentView().GetAnimation(ImageGalleryViewAnimationClose)
                If animation IsNot Nothing Then
                    Dim item = ImagesGridView.Items.FirstOrDefault(Function(i) DirectCast(i, SampleImage).ID = selectedImageId)
                    ImagesGridView.ScrollIntoView(item)
                    Await ImagesGridView.TryStartConnectedAnimationAsync(animation, item, "galleryImage")
                End If

                ApplicationData.Current.LocalSettings.SaveString(ImageGalleryViewSelectedIdKey, String.Empty)
            End If
        End Sub
    End Class
End Namespace
