Imports System.Collections.Generic
Imports Windows.Foundation
Imports Windows.UI.Input.Inking
Imports Param_ItemNamespace.EventHandlers.Ink

Namespace Services.Ink.UndoRedo
    Public Class MoveStrokesUndoRedoOperation
        Inherits IUndoRedoOperation

        Private ReadOnly _endPosition As Point
        Private ReadOnly _startPosition As Point
        Private ReadOnly _strokes As List(Of InkStroke)
        Private ReadOnly _strokeService As InkStrokesService

        Public Sub New(strokes As IEnumerable(Of InkStroke), startPosition As Point, endPosition As Point, strokeService As InkStrokesService)
            _strokes = New List(Of InkStroke)(strokes)
            _startPosition = startPosition
            _endPosition = endPosition
            _strokeService = strokeService
            _strokeService.AddStrokeEvent += AddressOf StrokeService_AddStrokeEvent
        End Sub

        Public Sub ExecuteRedo()
            _strokeService.SelectStrokes(_strokes)
            _strokeService.MoveSelectedStrokes(_startPosition, _endPosition)
        End Sub

        Public Sub ExecuteUndo()
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
