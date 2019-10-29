Imports Microsoft.Xaml.Interactivity
Imports WinUI = Microsoft.UI.Xaml.Controls

Namespace Behaviors
    Public Class TreeViewCollapseBehavior
        Inherits Behavior(Of WinUI.TreeView)

        Public ReadOnly Property CollapseAllCommand As ICommand

        Public Sub New()
            CollapseAllCommand = New RelayCommand(Sub()
                    CollapseNodes(AssociatedObject.RootNodes)
                End Sub)
        End Sub

        Private Sub CollapseNodes(nodes As IList(Of WinUI.TreeViewNode))
            For Each node In nodes
                CollapseNodes(node.Children)
                AssociatedObject.Collapse(node)
            Next
        End Sub
    End Class
End Namespace