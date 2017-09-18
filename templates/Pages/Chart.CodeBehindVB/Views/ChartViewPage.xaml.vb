Imports Windows.UI.Xaml.Controls
Imports System.Collections.ObjectModel
Imports Param_ItemNamespace.Models
Imports Param_ItemNamespace.Services
Imports System.ComponentModel

Namespace Views
    Partial NotInheritable Class ChartViewPage
        Inherits Page
        Implements INotifyPropertyChanged

        ' TODO: UWPTemplates: Change the chart as appropriate to your app.
        ' For help see http://docs.telerik.com/windows-universal/controls/radchart/getting-started

        Public ReadOnly Property Source As ObservableCollection(Of DataPoint)
            Get
                ' TODO WTS: Replace this with your actual data
                Return SampleDataService.GetChartSampleData()
            End Get
        End Property

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    End Class
End Namespace
