Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services

Namespace Views
    Public NotInheritable Partial Class ChartViewPage
        Inherits Page
        Implements System.ComponentModel.INotifyPropertyChanged

        Private _source As ObservableCollection(Of DataPoint)

        ' TODO WTS: Change the chart as appropriate to your app.
        ' For help see http://docs.telerik.com/windows-universal/controls/radchart/getting-started
        Public Sub New()
            InitializeComponent()
        End Sub

        Public Property Source As ObservableCollection(Of DataPoint)
            Get
                Return _source
            End Get
            Set(value As ObservableCollection(Of DataPoint))
                [Set](_source, value)
            End Set
        End Property

        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            ' TODO WTS: Replace this with your actual data
            Source = Await SampleDataService.GetChartDataAsync()
        End Sub
    End Class
End Namespace
