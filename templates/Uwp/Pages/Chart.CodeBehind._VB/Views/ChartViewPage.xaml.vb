Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services

Namespace Views
    Public NotInheritable Partial Class ChartViewPage
        Inherits Page
        Implements System.ComponentModel.INotifyPropertyChanged

        Public Property Source As ObservableCollection(Of DataPoint) = New ObservableCollection(Of DataPoint)

        ' TODO WTS: Change the chart as appropriate to your app.
        ' For help see http://docs.telerik.com/windows-universal/controls/radchart/getting-started
        Public Sub New()
            InitializeComponent()
        End Sub

        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            Source.Clear()

            ' TODO WTS: Replace this with your actual data
            Dim data = Await SampleDataService.GetChartDataAsync()

            For Each item As DataPoint In data
                Source.Add(item)
            Next
        End Sub
    End Class
End Namespace
