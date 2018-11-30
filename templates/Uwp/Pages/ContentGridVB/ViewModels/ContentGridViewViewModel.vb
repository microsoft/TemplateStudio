Imports Param_ItemNamespace.Core.Models
Imports Param_ItemNamespace.Core.Services
Imports Param_ItemNamespace.Services
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

        Public ReadOnly Property ItemSelectedCommand As ICommand = New RelayCommand(Of ItemClickEventArgs)(Sub(args) OnsItemSelected(args))

        Private Sub OnsItemSelected(args As ItemClickEventArgs)
            Dim item = TryCast(args.ClickedItem, SampleOrder)
            If item IsNot Nothing Then
                NavigationService.Frame.SetListDataItemForNextConnectedAnnimation(item)
            End If
        End Sub
    End Class
End Namespace
