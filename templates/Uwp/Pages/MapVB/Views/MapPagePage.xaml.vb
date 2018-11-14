Namespace Views
    Public NotInheritable Partial Class MapPagePage
        Inherits Page

        Public Sub New()
            InitializeComponent()
        End Sub

        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            Await ViewModel.InitializeAsync(mapControl)
        End Sub

        Protected Overrides Sub OnNavigatedFrom(e As NavigationEventArgs)
            ViewModel.Cleanup()
        End Sub
    End Class
End Namespace
