Imports System.Linq
Imports Param_RootNamespace.Helpers
Imports Param_RootNamespace.Models
Imports WinUI = Microsoft.UI.Xaml.Controls

Namespace ViewModels
    Public Class wts.ItemNameViewModel
        Inherits System.ComponentModel.INotifyPropertyChanged

        Private _addTabCommand As RelayCommand
        Private _closeTabCommand As RelayCommand(Of WinUI.TabViewTabCloseRequestedEventArgs)

        Public ReadOnly Property AddTabCommand As RelayCommand
            Get
                If _addTabCommand Is Nothing Then
                    _addTabCommand = New RelayCommand(AddressOf AddTab)
                End If

                Return _addTabCommand
            End Get
        End Property

        Public ReadOnly Property CloseTabCommand As RelayCommand(Of WinUI.TabViewTabCloseRequestedEventArgs)
            Get
                If _closeTabCommand Is Nothing Then
                    _closeTabCommand = New RelayCommand(Of WinUI.TabViewTabCloseRequestedEventArgs)(AddressOf CloseTab)
                End If

                Return _closeTabCommand
            End Get
        End Property

        ' In this sample the content shown in the Tab is a string, set the content to the model you want to show
        Public ReadOnly Property Tabs As ObservableCollection(Of TabViewItemData) = New ObservableCollection(Of TabViewItemData)() From {
            New TabViewItemData() With {
                .Index = 1,
                .Header = "Item 1",
                .Content = "This is the content for Item 1."
            },
            New TabViewItemData() With {
                .Index = 2,
                .Header = "Item 2",
                .Content = "This is the content for Item 2."
            },
            New TabViewItemData() With {
                .Index = 3,
                .Header = "Item 3",
                .Content = "This is the content for Item 3."
            }
        }

        Public Sub New()
        End Sub

        Private Sub AddTab()
            Dim newIndex As Integer = If(Tabs.Any(), Tabs.Max(Function(t) t.Index) + 1, 1)
            Tabs.Add(New TabViewItemData() With {
                .Index = newIndex,
                .Header = $"Item {newIndex}",
                .Content = $"This is the content for Item {newIndex}"
            })
        End Sub

        Private Sub CloseTab(args As WinUI.TabViewTabCloseRequestedEventArgs)
            Dim item As TabViewItemData = TryCast(args.Item, TabViewItemData)

            If item IsNot Nothing Then
                Tabs.Remove(item)
            End If
        End Sub
    End Class
End Namespace
