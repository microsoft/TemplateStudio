Imports Windows.System
Imports Microsoft.Toolkit.Uwp.UI.Animations

Imports Param_RootNamespace.Helpers
Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services
Imports Param_RootNamespace.Services

Namespace Views
    Public NotInheritable Partial Class ImageGalleryViewDetailPage
        Inherits Page
        Implements System.ComponentModel.INotifyPropertyChanged

        Private _selectedImage As Object

        Public Property SelectedImage As Object
            Get
                Return _selectedImage
            End Get
            Set
                [Param_Setter](_selectedImage, value)
                ImagesNavigationHelper.UpdateImageId(ImageGalleryViewPage.ImageGalleryViewSelectedIdKey, DirectCast(SelectedImage, SampleImage).ID)
            End Set
        End Property

        Public Property Source As ObservableCollection(Of SampleImage) = New ObservableCollection(Of SampleImage)

        Public Sub New()
            InitializeComponent()
        End Sub

        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            Source.Clear()

            ' TODO WTS: Replace this with your actual data
            Dim data = Await SampleDataService.GetImageGalleryDataAsync("ms-appx:///Assets")
            For Each item As SampleImage In data
                Source.Add(item)
            Next

            Dim selectedImageId = TryCast(e.Parameter, String)
            If Not String.IsNullOrEmpty(selectedImageId) AndAlso e.NavigationMode = NavigationMode.New Then
                SelectedImage = Source.FirstOrDefault(Function(i) i.ID = selectedImageId)
            Else
                selectedImageId = ImagesNavigationHelper.GetImageId(ImageGalleryViewPage.ImageGalleryViewSelectedIdKey)
                If Not String.IsNullOrEmpty(selectedImageId) Then
                    SelectedImage = Source.FirstOrDefault(Function(i) i.ID = selectedImageId)
                End If
            End If
        End Sub

        Protected Overrides Sub OnNavigatingFrom(e As NavigatingCancelEventArgs)
            MyBase.OnNavigatingFrom(e)
            If e.NavigationMode = NavigationMode.Back Then
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(SelectedImage)
                ImagesNavigationHelper.RemoveImageId(ImageGalleryViewPage.ImageGalleryViewSelectedIdKey)
            End If
        End Sub

        Private Sub OnPageKeyDown(sender As Object, e As KeyRoutedEventArgs)
            If e.Key = VirtualKey.Escape AndAlso NavigationService.CanGoBack Then
                NavigationService.GoBack()
                e.Handled = True
            End If
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Private Sub [Set](Of T)(ByRef storage As T, value As T, <CallerMemberName> Optional propertyName As String = Nothing)
            If Equals(storage, value) Then
                Return
            End If

            storage = value
            OnPropertyChanged(propertyName)
        End Sub

        Private Sub OnPropertyChanged(propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub
    End Class
End Namespace
