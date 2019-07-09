Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services
Imports Param_RootNamespace.Services
Imports Microsoft.Toolkit.Uwp.UI.Animations

Namespace Views
    Public NotInheritable Partial Class ContentGridViewPage
        Inherits Page
        Implements INotifyPropertyChanged

        Public Property Source As ObservableCollection(Of SampleOrder) = New ObservableCollection(Of SampleOrder)

        Public Sub New()
            InitializeComponent()
        End Sub

        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            Source.Clear()

            ' TODO WTS: Replace this with your actual data
            Dim data = Await SampleDataService.GetContentGridDataAsync()

            For Each item As SampleOrder In data
                Source.Add(item)
            Next
        End Sub

        Private Sub OnItemClick(sender As Object, e As ItemClickEventArgs)
            Dim item = TryCast(e.ClickedItem, SampleOrder)
            If item IsNot Nothing Then
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(item)
                NavigationService.Navigate(Of ContentGridViewDetailPage)(item.OrderID)
            End If
        End Sub

    End Class
End Namespace
