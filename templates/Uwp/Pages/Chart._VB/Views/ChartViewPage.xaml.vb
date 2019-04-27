Namespace Views
    Public NotInheritable Partial Class ChartViewPage
        Inherits Page

        ' TODO WTS: Change the chart as appropriate to your app.
        ' For help see http://docs.telerik.com/windows-universal/controls/radchart/getting-started
        Public Sub New()
            InitializeComponent()
        End Sub

        Protected Overrides Async Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            Await ViewModel.LoadDataAsync()
        End Sub
    End Class
End Namespace
