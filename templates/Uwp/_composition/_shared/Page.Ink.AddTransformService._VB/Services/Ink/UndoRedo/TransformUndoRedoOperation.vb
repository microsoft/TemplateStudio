Imports Param_RootNamespace.EventHandlers.Ink

Namespace Services.Ink.UndoRedo
    Public Class TransformUndoRedoOperation
        Implements IUndoRedoOperation

        Private ReadOnly _strokeService As InkStrokesService
        Private _transformResult As InkTransformResult

        Public Sub New(transformResult As InkTransformResult, strokeService As InkStrokesService)
            _transformResult = transformResult
            _strokeService = strokeService
            AddHandler _strokeService.AddStrokeEvent, AddressOf StrokeService_AddStrokeEvent
        End Sub

        Public Sub ExecuteRedo() Implements IUndoRedoOperation.ExecuteRedo
            RemoveStrokes()
            AddTextAndShapes()
        End Sub

        Public Sub ExecuteUndo() Implements IUndoRedoOperation.ExecuteUndo
            RemoveTextAndShapes()
            AddStrokes()
        End Sub

        Private Sub StrokeService_AddStrokeEvent(sender As Object, e As AddStrokeEventArgs)
            If e.NewStroke Is Nothing Then
                Return
            End If

            Dim removedStrokes = _transformResult.Strokes.RemoveAll(Function(s) s.Id = e.OldStroke?.Id)

            If removedStrokes > 0 Then
                _transformResult.Strokes.Add(e.NewStroke)
            End If
        End Sub

        Private Sub AddTextAndShapes()
            For Each uielement In _transformResult.TextAndShapes.ToList()
                _transformResult.DrawingCanvas.Children.Add(uielement)
            Next
        End Sub

        Private Sub RemoveTextAndShapes()
            For Each uielement In _transformResult.TextAndShapes
                If _transformResult.DrawingCanvas.Children.Contains(uielement) Then
                    _transformResult.DrawingCanvas.Children.Remove(uielement)
                End If
            Next
        End Sub

        Private Sub AddStrokes()
            For Each stroke In _transformResult.Strokes.ToList()
                _strokeService.AddStroke(stroke)
            Next
        End Sub

        Private Sub RemoveStrokes()
            For Each stroke In _transformResult.Strokes
                _strokeService.RemoveStroke(stroke)
            Next
        End Sub
    End Class
End Namespace
