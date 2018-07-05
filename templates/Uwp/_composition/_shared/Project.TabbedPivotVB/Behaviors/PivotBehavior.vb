Imports System
Imports Microsoft.Xaml.Interactivity
Imports wts.ItemName.Helpers

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
            Dim removedItem = e.RemovedItems.Cast(Of PivotItem).Select(Function(pi) GetPivotPage(pi)).FirstOrDefault()
            Dim addedItem = e.AddedItems.Cast(Of PivotItem).Select(Function(pi) GetPivotPage(pi)).FirstOrDefault()
            If removedItem IsNot Nothing Then
                Await removedItem.OnPivotUnselectedAsync()
            End If

            If addedItem IsNot Nothing Then
                Await addedItem.OnPivotSelectedAsync()
            End If
        End Sub

        Private Function GetPivotPage(pivotItem As PivotItem) As IPivotPage
            Dim frame = TryCast(pivotItem.Content, Frame)
            If frame IsNot Nothing Then
                Return TryCast(frame.Content, IPivotPage)
            Else
                Return Nothing
            End If
        End Function
    End Class
End Namespace