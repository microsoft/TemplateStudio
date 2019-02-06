Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services

Namespace ViewModels
    Public Class MasterDetailViewViewModel
        Inherits System.ComponentModel.INotifyPropertyChanged

        Private _selected As SampleOrder

        Public Property Selected As SampleOrder
            Get
                Return _selected
            End Get
            Set
                [Param_Setter](_selected, value)
            End Set
        End Property

        Public Property SampleItems As ObservableCollection(Of SampleOrder) = new ObservableCollection(Of SampleOrder)

        Public Sub New()
        End Sub

        Public Async Function LoadDataAsync(viewState As MasterDetailsViewState) As Task
            SampleItems.Clear()

            Dim data = Await SampleDataService.GetSampleModelDataAsync()

            For Each item As SampleOrder In data
                SampleItems.Add(item)
            Next

            If viewState = MasterDetailsViewState.Both Then
                Selected = SampleItems.FirstOrDefault()
            End If
        End Function
    End Class
End Namespace
