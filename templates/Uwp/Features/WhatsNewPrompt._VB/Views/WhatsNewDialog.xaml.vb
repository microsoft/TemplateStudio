Namespace Views
    Public NotInheritable Partial Class WhatsNewDialog
        Inherits ContentDialog

        Public Sub New()
            ' TODO WTS: Update the contents of this dialog every time you release a new version of the app
            RequestedTheme =(TryCast(Window.Current.Content, FrameworkElement)).RequestedTheme
            InitializeComponent()
        End Sub
    End Class
End Namespace
