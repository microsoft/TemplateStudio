Imports Param_ItemNamespace.Core.Models
Imports Param_ItemNamespace.Core.Services

Namespace ViewModels
    Public Class DataGridViewViewModel
        Inherits System.ComponentModel.INotifyPropertyChanged

        Public ReadOnly Property Source As ObservableCollection(Of SampleOrder)
            Get
                ' TODO WTS: Replace this with your actual data
                Return SampleDataService.GetGridSampleData()
            End Get
        End Property
    End Class
End Namespace
