Imports Param_ItemNamespace.Core.Models
Imports Param_ItemNamespace.Core.Services
Imports Param_ItemNamespace.Services
Imports Microsoft.Toolkit.Uwp.UI.Animations

Namespace Views
    Public NotInheritable Partial Class ContentGridViewPage
        Inherits Page
        Implements INotifyPropertyChanged

        Public ReadOnly Property Source As ObservableCollection(Of SampleOrder)
            Get
                ' TODO WTS: Replace this with your actual data
                Return SampleDataService.GetContentGridData()
            End Get
        End Property

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub OnItemClick(sender As Object, e As ItemClickEventArgs)
            Dim item = TryCast(e.ClickedItem, SampleOrder)
            If item IsNot Nothing Then
                NavigationService.Frame.SetListDataItemForNextConnectedAnnimation(item)
                NavigationService.Navigate(Of ContentGridViewDetailPage)(item.OrderId)
            End If
        End Sub

    End Class
End Namespace
