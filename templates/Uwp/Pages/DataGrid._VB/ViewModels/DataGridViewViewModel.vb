Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services

Namespace ViewModels
    Public Class DataGridViewViewModel
        Inherits System.ComponentModel.INotifyPropertyChanged

        Private _source As ObservableCollection(Of SampleOrder)

        Public Property Source As ObservableCollection(Of SampleOrder)
            Get
                Return _source
            End Get
            Set(value As ObservableCollection(Of SampleOrder))
                [Set](_source, value)
            End Set
        End Property

        Public Async Function LoadDataAsync() As Task
            ' TODO WTS: Replace this with your actual data
            Source = Await SampleDataService.GetGridDataAsync()
        End Function
    End Class
End Namespace
