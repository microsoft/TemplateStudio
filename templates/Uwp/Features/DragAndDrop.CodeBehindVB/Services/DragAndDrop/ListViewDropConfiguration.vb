Imports Param_ItemNamespace.Models

Namespace Services.DragAndDrop

    Public Class ListViewDropConfiguration
        Inherits DropConfiguration

        Public Shared ReadOnly DragItemsStartingActionProperty As DependencyProperty = DependencyProperty.Register("DragItemsStartingAction", GetType(Action(Of DragDropStartingData)), GetType(DropConfiguration), New PropertyMetadata(Nothing))

        Public Shared ReadOnly DragItemsCompletedActionProperty As DependencyProperty = DependencyProperty.Register("DragItemsCompletedAction", GetType(Action(Of DragDropCompletedData)), GetType(DropConfiguration), New PropertyMetadata(Nothing))

        Public Property DragItemsStartingAction As Action(Of DragDropStartingData)
            Get
                Return CType(GetValue(DragItemsStartingActionProperty), Action(Of DragDropStartingData))
            End Get

            Set(value As Action(Of DragDropStartingData))
                SetValue(DragItemsStartingActionProperty, value)
            End Set
        End Property

        Public Property DragItemsCompletedAction As Action(Of DragDropCompletedData)
            Get
                Return CType(GetValue(DragItemsCompletedActionProperty), Action(Of DragDropCompletedData))
            End Get

            Set(value As Action(Of DragDropCompletedData))
                SetValue(DragItemsCompletedActionProperty, value)
            End Set
        End Property
    End Class
End Namespace
