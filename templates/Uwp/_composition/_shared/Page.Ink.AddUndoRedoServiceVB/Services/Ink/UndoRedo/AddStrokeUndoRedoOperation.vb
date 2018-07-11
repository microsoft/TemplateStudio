Imports System.Collections.Generic
Imports System.Linq
Imports Windows.UI.Input.Inking
Imports Param_ItemNamespace.EventHandlers.Ink

Namespace Services.Ink.UndoRedo
    Public Class AddStrokeUndoRedoOperation
        Inherits IUndoRedoOperation

        Private ReadOnly _strokeService As InkStrokesService
        Private ReadOnly _strokes As List(Of InkStroke)

        Public Sub New(strokes As IEnumerable(Of InkStroke), strokeService As InkStrokesService)
            _strokes = New List(Of InkStroke)(strokes)
            _strokeService = strokeService
            _strokeService.AddStrokeEvent += AddressOf StrokeService_AddStrokeEvent
        End Sub

        Public Sub ExecuteUndo()
            Return _strokes.ForEach(Sub(s) _strokeService.RemoveStroke(s))
        End Sub

        Public Sub ExecuteRedo()
            Return _strokes.ToList().ForEach(Sub(s) _strokeService.AddStroke(s))
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
