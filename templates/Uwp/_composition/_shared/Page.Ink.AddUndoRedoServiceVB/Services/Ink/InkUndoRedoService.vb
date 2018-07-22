Imports System.Collections.Generic
Imports System.Linq
Imports Windows.UI.Xaml.Controls
Imports Param_ItemNamespace.EventHandlers.Ink
Imports Param_ItemNamespace.Services.Ink.UndoRedo

Namespace Services.Ink
    Public Class InkUndoRedoService
        Private ReadOnly _strokeService As InkStrokesService
        Private undoStack As Stack(Of IUndoRedoOperation) = New Stack(Of IUndoRedoOperation)()
        Private redoStack As Stack(Of IUndoRedoOperation) = New Stack(Of IUndoRedoOperation)()

        Public Sub New(inkCanvas As InkCanvas, strokeService As InkStrokesService)
            _strokeService = strokeService
            AddHandler _strokeService.MoveStrokesEvent, AddressOf StrokeService_MoveStrokesEvent
            AddHandler _strokeService.CutStrokesEvent, AddressOf StrokeService_CutStrokesEvent
            AddHandler _strokeService.PasteStrokesEvent, AddressOf StrokeService_PasteStrokesEvent
            AddHandler inkCanvas.InkPresenter.StrokesCollected, Sub(s, e) AddOperation(New AddStrokeUndoRedoOperation(e.Strokes, _strokeService))
            AddHandler inkCanvas.InkPresenter.StrokesErased, Sub(s, e) AddOperation(New RemoveStrokeUndoRedoOperation(e.Strokes, _strokeService))
        End Sub

        Public Sub Reset()
            undoStack.Clear()
            redoStack.Clear()
        End Sub

        Public ReadOnly Property CanUndo As Boolean
            Get
                Return undoStack.Any()
            End Get
        End Property

        Public ReadOnly Property CanRedo As Boolean
            Get
                Return redoStack.Any()
            End Get
        End Property

        Public Sub Undo()
            If Not CanUndo Then
                Return
            End If

            RemoveHandler _strokeService.MoveStrokesEvent, AddressOf StrokeService_MoveStrokesEvent
            Dim element = undoStack.Pop()
            element.ExecuteUndo()
            redoStack.Push(element)
            AddHandler _strokeService.MoveStrokesEvent, AddressOf StrokeService_MoveStrokesEvent
        End Sub

        Public Sub Redo()
            If Not CanRedo Then
                Return
            End If

            RemoveHandler _strokeService.MoveStrokesEvent, AddressOf StrokeService_MoveStrokesEvent
            Dim element = redoStack.Pop()
            element.ExecuteRedo()
            undoStack.Push(element)
            AddHandler _strokeService.MoveStrokesEvent, AddressOf StrokeService_MoveStrokesEvent
        End Sub

        Public Sub AddOperation(operation As IUndoRedoOperation)
            If operation Is Nothing Then
                Return
            End If

            undoStack.Push(operation)
            redoStack.Clear()
        End Sub

        Private Sub StrokeService_MoveStrokesEvent(sender As Object, e As MoveStrokesEventArgs)
            Dim operation = New MoveStrokesUndoRedoOperation(e.Strokes, e.StartPosition, e.EndPosition, _strokeService)
            AddOperation(operation)
        End Sub

        Private Sub StrokeService_CutStrokesEvent(sender As Object, e As CopyPasteStrokesEventArgs)
            Dim operation = New RemoveStrokeUndoRedoOperation(e.Strokes, _strokeService)
            AddOperation(operation)
        End Sub

        Private Sub StrokeService_PasteStrokesEvent(sender As Object, e As CopyPasteStrokesEventArgs)
            Dim operation = New AddStrokeUndoRedoOperation(e.Strokes, _strokeService)
            AddOperation(operation)
        End Sub
    End Class
End Namespace
