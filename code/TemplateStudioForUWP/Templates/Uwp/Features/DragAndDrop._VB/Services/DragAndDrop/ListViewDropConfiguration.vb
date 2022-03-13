Namespace Services.DragAndDrop

    Public Class ListViewDropConfiguration
        Inherits DropConfiguration

        Public Shared ReadOnly DragItemsStartingCommandProperty As DependencyProperty = DependencyProperty.Register("DragItemsStartingCommand", GetType(ICommand), GetType(DropConfiguration), New PropertyMetadata(Nothing))

        Public Shared ReadOnly DragItemsCompletedCommandProperty As DependencyProperty = DependencyProperty.Register("DragItemsCompletedCommand", GetType(ICommand), GetType(DropConfiguration), New PropertyMetadata(Nothing))

        Public Property DragItemsStartingCommand As ICommand
            Get
                Return CType(GetValue(DragItemsStartingCommandProperty), ICommand)
            End Get

            Set(value As ICommand)
                SetValue(DragItemsStartingCommandProperty, value)
            End Set
        End Property

        Public Property DragItemsCompletedCommand As ICommand
            Get
                Return CType(GetValue(DragItemsCompletedCommandProperty), ICommand)
            End Get

            Set(value As ICommand)
                SetValue(DragItemsCompletedCommandProperty, value)
            End Set
        End Property
    End Class
End Namespace
