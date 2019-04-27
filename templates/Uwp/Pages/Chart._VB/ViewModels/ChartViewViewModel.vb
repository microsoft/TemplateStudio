Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services

Namespace ViewModels
    Public Class ChartViewViewModel
        Inherits System.ComponentModel.INotifyPropertyChanged

        Private _source As ObservableCollection(Of DataPoint)

        Public Sub New()
        End Sub

        Public Property Source As ObservableCollection(Of DataPoint)
            Get
                Return _source
            End Get
            Set(value As ObservableCollection(Of DataPoint))
                [Param_Setter](_source, value)
            End Set
        End Property

        Public Async Function LoadDataAsync() As Task
            Source = Await SampleDataService.GetChartSampleDataAsync()
        End Function
    End Class
End Namespace
