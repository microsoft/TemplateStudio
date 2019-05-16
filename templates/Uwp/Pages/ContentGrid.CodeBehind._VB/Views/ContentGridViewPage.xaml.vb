Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services
Imports Param_RootNamespace.Services
Imports Microsoft.Toolkit.Uwp.UI.Animations

Namespace Views
    Public NotInheritable Partial Class ContentGridViewPage
        Inherits Page
        Implements INotifyPropertyChanged

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
            InitializeComponent()
        End Sub

        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)

            ' TODO WTS: Replace this with your actual data
            Source = Await SampleDataService.GetContentGridDataAsync()
        End Sub

        Private Sub OnItemClick(sender As Object, e As ItemClickEventArgs)
            Dim item = TryCast(e.ClickedItem, SampleOrder)
            If item IsNot Nothing Then
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(item)
                NavigationService.Navigate(Of ContentGridViewDetailPage)(item.OrderId)
            End If
        End Sub

    End Class
End Namespace
