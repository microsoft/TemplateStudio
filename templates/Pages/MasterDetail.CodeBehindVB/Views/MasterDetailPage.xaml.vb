Imports Windows.UI.Xaml.Controls
Imports Param_ItemNamespace.Models
Imports Param_ItemNamespace.Services

Namespace Views
    Public NotInheritable Partial Class MasterDetailPage
        Inherits Page
        Implements System.ComponentModel.INotifyPropertyChanged

        Private _selected As SampleOrder

        Public Property Selected As SampleOrder
            Get
                Return _selected
            End Get
            Set
                [Set](_selected, value)
            End Set
        End Property

        Public Property SampleItems As ObservableCollection(Of SampleOrder)  = new ObservableCollection(Of SampleOrder)

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Async Function LoadDataAsync() As Task
            SampleItems.Clear()

            Dim data = Await SampleDataService.GetSampleModelDataAsync()

            For Each item As SampleOrder In data
                SampleItems.Add(item)
            Next

            Selected = SampleItems.First()
        End Function

        private Sub MasterListView_ItemClick(sender As Object, args As ItemClickEventArgs)
            Dim item As SampleOrder = TryCast(args?.ClickedItem, SampleOrder)
            If item IsNot Nothing Then
                If WindowStates.CurrentState.Equals(NarrowState) Then
                    NavigationService.Navigate(GetType(MasterDetailDetailPage), item)
                Else
                    Selected = item
                End If
            End If
        End Sub

    End Class
End Namespace

