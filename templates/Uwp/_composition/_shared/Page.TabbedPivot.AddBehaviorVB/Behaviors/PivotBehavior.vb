Imports System
Imports Param_ItemNamespace.Helpers
Imports Microsoft.Xaml.Interactivity

Namespace Behaviors
    Public Class PivotBehavior
        Inherits Behavior(Of Pivot)

        Protected Overrides Sub OnAttached()
            MyBase.OnAttached()
            AddHandler AssociatedObject.SelectionChanged, AddressOf OnSelectionChanged
        End Sub

        Protected Overrides Sub OnDetaching()
            MyBase.OnDetaching()
            RemoveHandler AssociatedObject.SelectionChanged, AddressOf OnSelectionChanged
        End Sub

        Private Async Sub OnSelectionChanged(sender As Object, e As SelectionChangedEventArgs)
            Dim removedItem = e.RemovedItems.Cast(Of PivotItem).Select(Function(pi) pi.GetPage(Of IPivotPage)).FirstOrDefault()
            Dim addedItem = e.AddedItems.Cast(Of PivotItem).Select(Function(pi) pi.GetPage(Of IPivotPage)).FirstOrDefault()
            If removedItem IsNot Nothing Then
                Await removedItem.OnPivotUnselectedAsync()
            End If

            If addedItem IsNot Nothing Then
                Await addedItem.OnPivotSelectedAsync()
            End If
        End Sub
    End Class
End Namespace
