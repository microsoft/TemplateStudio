Namespace Views
    Public NotInheritable Partial Class ImageGalleryViewPage
        Inherits Page

        Public Sub New()
            InitializeComponent()
            ViewModel.Initialize(gridView)
        End Sub        
    End Class
End Namespace
