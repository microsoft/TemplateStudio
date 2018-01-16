Imports Windows.Storage
Imports Windows.UI.Xaml.Media.Animation

Imports Param_ItemNamespace.Helpers
Imports Param_ItemNamespace.Models
Imports Param_ItemNamespace.Services

Namespace Views
    Public NotInheritable Partial Class ImageGalleryViewDetailPage
        Inherits Page
        Implements System.ComponentModel.INotifyPropertyChanged

        Private _timer As New DispatcherTimer() With {
             .Interval = TimeSpan.FromMilliseconds(500)
        }

        Private _selectedImage As Object

        Private _source As ObservableCollection(Of SampleImage)

        Public Property SelectedImage As Object
            Get
                Return _selectedImage
            End Get
            Set
                [Param_Setter](_selectedImage, value)
                ApplicationData.Current.LocalSettings.SaveString(ImageGalleryViewPage.ImageGalleryViewSelectedIdKey, DirectCast(SelectedImage, SampleImage).ID)
            End Set
        End Property

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

        Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            Dim sampleImage = TryCast(e.Parameter, SampleImage)
            SelectedImage = Source.FirstOrDefault(Function(i) i.ID = sampleImage.ID)
            Dim animation = ConnectedAnimationService.GetForCurrentView().GetAnimation(ImageGalleryViewPage.ImageGalleryViewAnimationOpen)
            animation?.TryStart(previewImage)
            showFlipView.Begin()
        End Sub

        Protected Overrides Sub OnNavigatingFrom(e As NavigatingCancelEventArgs)
            MyBase.OnNavigatingFrom(e)
            If e.NavigationMode = NavigationMode.Back Then
                previewImage.Visibility = Visibility.Visible
                ConnectedAnimationService.GetForCurrentView()?.PrepareToAnimate(ImageGalleryViewPage.ImageGalleryViewAnimationClose, previewImage)
            End If
        End Sub
    End Class
End Namespace
