Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services
Imports Param_RootNamespace.Services
Imports Microsoft.Toolkit.Uwp.UI.Animations

Namespace ViewModels
    Public Class ContentGridViewViewModel
        Inherits System.ComponentModel.INotifyPropertyChanged

        Public ReadOnly Property Source As ObservableCollection(Of SampleOrder)
            Get
                ' TODO WTS: Replace this with your actual data
                Return SampleDataService.GetContentGridData()
            End Get
        End Property

        Public Sub New()
        End Sub

        Public ReadOnly Property ItemClickCommand  As ICommand = New RelayCommand(Of SampleOrder)(Sub(order) OnItemClick(order))

        Private Sub OnItemClick(clickedItem As SampleOrder)
            If clickedItem IsNot Nothing Then
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(clickedItem)
            End If
        End Sub
    End Class
End Namespace
