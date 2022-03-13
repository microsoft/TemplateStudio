Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services
Imports Param_RootNamespace.Services
Imports Microsoft.Toolkit.Uwp.UI.Animations

Namespace Views
    Public NotInheritable Partial Class ContentGridViewDetailPage
        Inherits Page
        Implements INotifyPropertyChanged

        Private _item As SampleOrder

        Public Property Item As SampleOrder
            Get
                Return _item
            End Get
            Set(value As SampleOrder)
                [Set](_item, value)
            End Set
        End Property

        Public Sub New()
            InitializeComponent()
        End Sub

        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            RegisterElementForConnectedAnimation("animationKeyContentGridView", itemHero)
            Dim orderID As Long
            orderID = CType(e.Parameter, Long)
            Dim data = Await SampleDataService.GetContentGridDataAsync()
            Item = data.First(Function(i) i.OrderID = orderID)
        End Sub

        Protected Overrides Sub OnNavigatingFrom(e As NavigatingCancelEventArgs)
            MyBase.OnNavigatingFrom(e)

            If e.NavigationMode = NavigationMode.Back Then
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(Item)
            End If
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Private Sub [Set](Of T)(ByRef storage As T, value As T, <CallerMemberName> Optional propertyName As String = Nothing)
            If Equals(storage, value) Then
                Return
            End If

            storage = value
            OnPropertyChanged(propertyName)
        End Sub

        Private Sub OnPropertyChanged(propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub
    End Class
End Namespace
