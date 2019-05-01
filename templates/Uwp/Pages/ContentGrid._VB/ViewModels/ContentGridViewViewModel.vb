Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services
Imports Param_RootNamespace.Services
Imports Microsoft.Toolkit.Uwp.UI.Animations

Namespace ViewModels
    Public Class ContentGridViewViewModel
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

        Public Sub New()
        End Sub

        Public Async Function LoadDataAsync() As Task
            ' TODO WTS: Replace this with your actual data
            Source = Await SampleDataService.GetContentGridDataAsync()
        End Function

        Public ReadOnly Property ItemClickCommand  As ICommand = New RelayCommand(Of SampleOrder)(Sub(order) OnItemClick(order))

        Private Sub OnItemClick(clickedItem As SampleOrder)
            If clickedItem IsNot Nothing Then
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(clickedItem)
            End If
        End Sub
    End Class
End Namespace
