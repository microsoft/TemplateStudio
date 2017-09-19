Imports Windows.UI.Xaml.Controls

Namespace Views
    NotInheritable Class WebViewPagePage
        Inherits Page
        Public Sub New()
            InitializeComponent()
            ViewModel.Initialize(webView)
        End Sub
    End Class
End Namespace
