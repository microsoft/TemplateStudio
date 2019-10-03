Imports WinUI = Microsoft.UI.Xaml.Controls

Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services

Namespace Views
    ' For more info about the TreeView Control see
    ' https://docs.microsoft.com/windows/uwp/design/controls-and-patterns/tree-view
    ' For other samples, get the XAML Controls Gallery app http://aka.ms/XamlControlsGallery
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page
        Implements INotifyPropertyChanged

        Private _selectedItem As Object

        Public Property SelectedItem As Object
            Get
                Return _selectedItem
            End Get
            Set
                [Set](_selectedItem, Value)
            End Set
        End Property

        Public Property SampleItems As ObservableCollection(Of SampleCompany) = New ObservableCollection(Of SampleCompany)

        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            Dim data = Await SampleDataService.GetTreeViewDataAsync()
            For Each item In data
                SampleItems.Add(item)
            Next
        End Sub

        Private Sub OnItemInvoked(sender As WinUI.TreeView, args As WinUI.TreeViewItemInvokedEventArgs)
            SelectedItem = args.InvokedItem
        End Sub

        Private Sub OnCollapseAll(sender As Object, e As RoutedEventArgs)
            CollapseNodes(treeView.RootNodes)
        End Sub

        Private Sub CollapseNodes(nodes As IList(Of WinUI.TreeViewNode))
            For Each node In nodes
                CollapseNodes(node.Children)
                treeView.Collapse(node)
            Next
        End Sub

    End Class
End Namespace