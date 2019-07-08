Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services
Imports Param_RootNamespace.Helpers
Imports WinUI = Microsoft.UI.Xaml.Controls

Namespace ViewModels
    Public Class wts.ItemNameViewModel
        Inherits System.ComponentModel.INotifyPropertyChanged

        Private _selectedItem As Object
        Private _itemInvokedCommand As ICommand

        Public Property SelectedItem As Object
            Get
                Return _selectedItem
            End Get
            Set
                [Param_Setter](_selectedItem, Value)
            End Set
        End Property

        Public Property SampleItems As ObservableCollection(Of SampleCompany) = New ObservableCollection(Of SampleCompany)

        Public ReadOnly Property ItemInvokedCommand As ICommand
            Get
                If _itemInvokedCommand Is Nothing Then
                    _itemInvokedCommand = New RelayCommand(Of WinUI.TreeViewItemInvokedEventArgs)(AddressOf OnItemInvoked)
                End If

                Return _itemInvokedCommand
            End Get
        End Property

        Public Sub New()
        End Sub

        Public Async Function LoadDataAsync() As Task
            Dim data = Await SampleDataService.GetTreeViewDataAsync()
            For Each item In data
                SampleItems.Add(item)
            Next
        End Function

        Private Sub OnItemInvoked(args As WinUI.TreeViewItemInvokedEventArgs)
            SelectedItem = args.InvokedItem
        End Sub
    End Class
End Namespace