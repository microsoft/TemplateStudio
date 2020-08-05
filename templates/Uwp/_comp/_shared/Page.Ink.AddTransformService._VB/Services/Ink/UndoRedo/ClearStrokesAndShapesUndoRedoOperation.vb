Imports Param_RootNamespace.EventHandlers.Ink
Imports System.Collections.Generic
Imports System.Linq
Imports Windows.UI.Input.Inking

Namespace Services.Ink.UndoRedo
    Public Class ClearStrokesAndShapesUndoRedoOperation
        Implements IUndoRedoOperation

        Private ReadOnly _strokeService As InkStrokesService
        Private ReadOnly _transformService As InkTransformService
        Private _strokes As List(Of InkStroke)
        Private _textAndShapes As List(Of UIElement)

        Public Sub New(strokes As IEnumerable(Of InkStroke), textAndShapes As IEnumerable(Of UIElement), strokeService As InkStrokesService, transformService As InkTransformService)
            _strokes = New List(Of InkStroke)(strokes)
            _textAndShapes = New List(Of UIElement)(textAndShapes)
            _strokeService = strokeService
            _transformService = transformService
            AddHandler _strokeService.AddStrokeEvent, AddressOf StrokeService_AddStrokeEvent
        End Sub

        Public Sub ExecuteRedo() Implements IUndoRedoOperation.ExecuteRedo
            _strokes.ForEach(Sub(s) _strokeService.RemoveStroke(s))
            _textAndShapes.ForEach(Sub(s) _transformService.RemoveUIElement(s))
        End Sub

        Public Sub ExecuteUndo() Implements IUndoRedoOperation.ExecuteUndo
            _strokes.ToList().ForEach(Sub(s) _strokeService.AddStroke(s))
            _textAndShapes.ToList().ForEach(Sub(s) _transformService.AddUIElement(s))
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
