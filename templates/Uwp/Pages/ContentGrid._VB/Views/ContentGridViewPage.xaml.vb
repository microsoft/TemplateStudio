Namespace Views
    Public NotInheritable Partial Class ContentGridViewPage
        Inherits Page

        Public Sub New()
            InitializeComponent()
        End Sub

        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)

            Await ViewModel.LoadDataAsync()
        End Sub
    End Class
End Namespace
