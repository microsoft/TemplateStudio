Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services

Namespace ViewModels
    Public Class ChartViewViewModel
        Inherits System.ComponentModel.INotifyPropertyChanged

        Public Property Source As ObservableCollection(Of DataPoint) = New ObservableCollection(Of DataPoint)

        Public Sub New()
        End Sub

        Public Async Function LoadDataAsync() As Task
            Source.Clear()

            ' TODO WTS: Replace this with your actual data
            Dim data = Await SampleDataService.GetChartDataAsync()

            For Each item As DataPoint In data
                Source.Add(item)
            Next
        End Function
    End Class
End Namespace
