Namespace Views
    Public NotInheritable Partial Class ImageGalleryViewPage
        Inherits Page

        Public Sub New()
            InitializeComponent()
            AddHandler Loaded, Async Sub(sender, eventArgs)
                                    ' TODO WTS: Replace this with your actual data
                                    Await ViewModel.LoadDataAsync()
                                End Sub
        End Sub
    End Class
End Namespace
