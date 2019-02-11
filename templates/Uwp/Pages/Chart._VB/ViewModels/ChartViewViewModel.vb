Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services

Namespace ViewModels
    Public Class ChartViewViewModel
        Inherits System.ComponentModel.INotifyPropertyChanged

        Public Sub New()
        End Sub

        Public ReadOnly Property Source As ObservableCollection(Of DataPoint)
            Get
                ' TODO WTS: Replace this with your actual data
                Return SampleDataService.GetChartSampleData()
            End Get
        End Property
    End Class
End Namespace
