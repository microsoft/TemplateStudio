Imports WinUI = Microsoft.UI.Xaml.Controls
Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services

Namespace Views
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page
        Implements INotifyPropertyChanged

        Private _selected As SampleOrder
        Private _twoPanePriority As WinUI.TwoPaneViewPriority

        Public Property Selected As SampleOrder
            Get
                Return _selected
            End Get
            Set(value As SampleOrder)
                [Set](_selected, value)
            End Set
        End Property

        Public Property TwoPanePriority As WinUI.TwoPaneViewPriority
            Get
                Return _twoPanePriority
            End Get
            Set(value As WinUI.TwoPaneViewPriority)
                [Set](_twoPanePriority, value)
            End Set
        End Property

        Public Property SampleItems As ObservableCollection(Of SampleOrder) = New ObservableCollection(Of SampleOrder)()

        Public Sub New()
            InitializeComponent()
        End Sub

        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            SampleItems.Clear()
            Dim data = Await SampleDataService.GetTwoPaneViewDataAsync()

            For Each item In data
                SampleItems.Add(item)
            Next

            Selected = SampleItems.First()
        End Sub

        Public Function TryCloseDetail() As Boolean
            If TwoPanePriority = WinUI.TwoPaneViewPriority.Pane2 Then
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane1
                Return True
            End If

            Return False
        End Function

        Private Sub OnItemClick(sender As Object, e As ItemClickEventArgs)
            If twoPaneView.Mode = WinUI.TwoPaneViewMode.SinglePane Then
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane2
            End If
        End Sub

        Private Sub OnModeChanged(sender As WinUI.TwoPaneView, args As Object)
            If twoPaneView.Mode = WinUI.TwoPaneViewMode.SinglePane Then
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane2
            Else
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane1
            End If
        End Sub
    End Class
End Namespace

