Imports Windows.UI.Input.Inking
Imports Param_RootNamespace.EventHandlers.Ink

Namespace Services.Ink.UndoRedo
    Public Class RemoveStrokeUndoRedoOperation
        Implements IUndoRedoOperation

        Private ReadOnly _strokeService As InkStrokesService
        Private _strokes As List(Of InkStroke)

        Public Sub New(strokes As IEnumerable(Of InkStroke), strokeService As InkStrokesService)
            _strokes = New List(Of InkStroke)(strokes)
            _strokeService = strokeService
            AddHandler _strokeService.AddStrokeEvent, AddressOf StrokeService_AddStrokeEvent
        End Sub

        Public Sub ExecuteRedo() Implements IUndoRedoOperation.ExecuteRedo
            _strokes.ForEach(Sub(s) _strokeService.RemoveStroke(s))
        End Sub

        Public Sub ExecuteUndo() Implements IUndoRedoOperation.ExecuteUndo
            _strokes.ToList().ForEach(Sub(s) _strokeService.AddStroke(s))
        End Sub

        Private Sub StrokeService_AddStrokeEvent(sender As Object, e As AddStrokeEventArgs)
            If e.NewStroke Is Nothing Then
                Return
            End If

            Dim changes = _strokes.RemoveAll(Function(s) s.Id = e.OldStroke?.Id)

            If changes > 0 Then
                _strokes.Add(e.NewStroke)
            End If
        End Sub
    End Class
End Namespace
