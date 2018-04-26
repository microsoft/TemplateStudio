Namespace Views
    Public NotInheritable Partial Class WebViewPagePage
        Inherits Page

        Public Sub New()
            InitializeComponent()
            ViewModel.Initialize(webView)
        End Sub
    End Class
End Namespace
