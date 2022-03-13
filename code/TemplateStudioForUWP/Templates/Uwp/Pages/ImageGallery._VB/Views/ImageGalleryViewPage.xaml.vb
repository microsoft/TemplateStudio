Namespace Views
    Public NotInheritable Partial Class ImageGalleryViewPage
        Inherits Page

        Public Sub New()
            InitializeComponent()
            AddHandler Loaded, AddressOf ImageGalleryViewPage_Loaded
        End Sub

        Public Async Sub ImageGalleryViewPage_Loaded(sender As Object, eventArgs As RoutedEventArgs)
            Await ViewModel.LoadDataAsync()
        End Sub
    End Class
End Namespace
