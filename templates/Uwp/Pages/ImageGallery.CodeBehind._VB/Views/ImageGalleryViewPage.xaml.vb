Imports Microsoft.Toolkit.Uwp.UI.Animations

Imports Param_RootNamespace.Helpers
Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services
Imports Param_RootNamespace.Services

Namespace Views
    Public NotInheritable Partial Class ImageGalleryViewPage
        Inherits Page
        Implements System.ComponentModel.INotifyPropertyChanged

        Public Const ImageGalleryViewSelectedIdKey As String = "ImageGalleryViewSelectedIdKey"

        Public Property Source As ObservableCollection(Of SampleImage) = New ObservableCollection(Of SampleImage)

        Public Sub New()
            InitializeComponent()
            AddHandler Loaded, AddressOf ImageGalleryViewPage_OnLoaded
        End Sub

        Public Async Sub ImageGalleryViewPage_OnLoaded(sender As Object, eventArgs As RoutedEventArgs)
            Source.Clear()

            ' TODO WTS: Replace this with your actual data
            Dim data = Await SampleDataService.GetImageGalleryDataAsync("ms-appx:///Assets")
            For Each item As SampleImage In data
                Source.Add(item)
            Next
        End Sub

        Private Sub ImagesGridView_ItemClick(sender As Object, e As ItemClickEventArgs)
            Dim selected = TryCast(e.ClickedItem, SampleImage)
            ImagesNavigationHelper.AddImageId(ImageGalleryViewSelectedIdKey, selected.ID)
            NavigationService.Frame.SetListDataItemForNextConnectedAnimation(selected)
            NavigationService.Navigate(Of ImageGalleryViewDetailPage)(selected.ID)
        End Sub
    End Class
End Namespace
