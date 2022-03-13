Namespace Services.DragAndDrop

    Public Class DragDropService

        Private Shared Sub ConfigureUIElement(element As UIElement, configuration As DropConfiguration)
            '{[{
            AddHandler element.DragEnter, Sub(sender, args)
                ' Operation is copy by default
                args.AcceptedOperation = DataPackageOperation.Copy
                Dim data = New DragDropData With {.AcceptedOperation = args.AcceptedOperation, .DataView = args.DataView}
                configuration.DragEnterAction?.Invoke(data)
                args.AcceptedOperation = data.AcceptedOperation
            End Sub
            AddHandler element.DragOver, Sub(sender, args)
                Dim data = New DragDropData With {.AcceptedOperation = args.AcceptedOperation, .DataView = args.DataView}
                configuration.DragOverAction?.Invoke(data)
                args.AcceptedOperation = data.AcceptedOperation
            End Sub
            AddHandler element.DragLeave, Sub(sender, args)
                Dim data = New DragDropData With {.AcceptedOperation = args.AcceptedOperation, .DataView = args.DataView}
                configuration.DragLeaveAction?.Invoke(data)
            End Sub
            AddHandler element.Drop, Async Sub(sender, args)
                Await configuration.ProcessComandsAsync(args.DataView)
            End Sub
            '}]}
        End Sub

        Private Shared Sub ConfigureListView(listview As ListViewBase, configuration As ListViewDropConfiguration)
            '{[{
            AddHandler listview.DragItemsStarting, Sub(sender, args)
                Dim data = New DragDropStartingData With {.Data = args.Data, .Items = args.Items}
                configuration.DragItemsStartingAction?.Invoke(data)
            End Sub
            AddHandler listview.DragItemsCompleted, Sub(sender, args)
                Dim data = New DragDropCompletedData With {.DropResult = args.DropResult, .Items = args.Items}
                configuration.DragItemsCompletedAction?.Invoke(data)
            End Sub
            '}]}
        End Sub
    End Class
End Namespace
