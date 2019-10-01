
Imports System.Linq
Imports System.Runtime.CompilerServices
Imports Param_RootNamespace.Models
Imports Windows.UI.Xaml.Controls
Imports WinUI = Microsoft.UI.Xaml.Controls

Namespace Views
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page
        Implements System.ComponentModel.INotifyPropertyChanged

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
            InitializeComponent()
        End Sub

        Private Sub OnAddTabButtonClick(sender As Microsoft.UI.Xaml.Controls.TabView, args As Object)
            Dim newIndex As Integer = If(Tabs.Any(), Tabs.Max(Function(t) t.Index) + 1, 1)
            Tabs.Add(New TabViewItemData() With {
                .Index = newIndex,
                .Header = $"Item {newIndex}",
                .Content = $"This is the content for Item {newIndex}"
            })
        End Sub

        Private Sub OnTabCloseRequested(sender As WinUI.TabView, args As WinUI.TabViewTabCloseRequestedEventArgs)
            Dim item As TabViewItemData = TryCast(args.Item, TabViewItemData)

            If item IsNot Nothing Then
                Tabs.Remove(item)
            End If
        End Sub
    End Class
End Namespace

