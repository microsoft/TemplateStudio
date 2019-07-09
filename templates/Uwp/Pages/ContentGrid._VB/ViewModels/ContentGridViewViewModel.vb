Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services
Imports Param_RootNamespace.Services
Imports Microsoft.Toolkit.Uwp.UI.Animations

Namespace ViewModels
    Public Class ContentGridViewViewModel
        Inherits System.ComponentModel.INotifyPropertyChanged

        Public Property Source As ObservableCollection(Of SampleOrder) = New ObservableCollection(Of SampleOrder)

        Public Sub New()
        End Sub

        Public Async Function LoadDataAsync() As Task
            Source.Clear()

            ' TODO WTS: Replace this with your actual data
            Dim data = Await SampleDataService.GetContentGridDataAsync()

            For Each item As SampleOrder In data
                Source.Add(item)
            Next
        End Function

        Public ReadOnly Property ItemClickCommand  As ICommand = New RelayCommand(Of SampleOrder)(Sub(order) OnItemClick(order))

        Private Sub OnItemClick(clickedItem As SampleOrder)
            If clickedItem IsNot Nothing Then
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(clickedItem)
            End If
        End Sub
    End Class
End Namespace
