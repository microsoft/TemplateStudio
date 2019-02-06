Imports Windows.UI.Input.Inking
Imports Param_RootNamespace.EventHandlers.Ink

Namespace Services.Ink.UndoRedo
    Public Class MoveStrokesUndoRedoOperation
        Implements IUndoRedoOperation

        Private ReadOnly _endPosition As Point
        Private ReadOnly _startPosition As Point
        Private ReadOnly _strokes As List(Of InkStroke)
        Private ReadOnly _strokeService As InkStrokesService

        Public Sub New(strokes As IEnumerable(Of InkStroke), startPosition As Point, endPosition As Point, strokeService As InkStrokesService)
            _strokes = New List(Of InkStroke)(strokes)
            _startPosition = startPosition
            _endPosition = endPosition
            _strokeService = strokeService
            AddHandler _strokeService.AddStrokeEvent, AddressOf StrokeService_AddStrokeEvent
        End Sub

        Public Sub ExecuteRedo() Implements IUndoRedoOperation.ExecuteRedo
            _strokeService.SelectStrokes(_strokes)
            _strokeService.MoveSelectedStrokes(_startPosition, _endPosition)
        End Sub

        Public Sub ExecuteUndo() Implements IUndoRedoOperation.ExecuteUndo
            _strokeService.SelectStrokes(_strokes)
            _strokeService.MoveSelectedStrokes(_endPosition, _startPosition)
        End Sub

        Private Sub StrokeService_AddStrokeEvent(sender As Object, e As AddStrokeEventArgs)
            If e.NewStroke Is Nothing Then
                Return
            End If

            Dim removedStrokes = _strokes.RemoveAll(Function(s) s.Id = e.OldStroke?.Id)

            If removedStrokes > 0 Then
                _strokes.Add(e.NewStroke)
            End If
        End Sub
    End Class
End Namespace
