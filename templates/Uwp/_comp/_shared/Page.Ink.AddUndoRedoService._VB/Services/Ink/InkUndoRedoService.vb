Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports Param_RootNamespace.EventHandlers.Ink
Imports Param_RootNamespace.Services.Ink.UndoRedo
Imports Windows.UI.Input.Inking

Namespace Services.Ink
    Public Class InkUndoRedoService
        Private ReadOnly _strokeService As InkStrokesService
        Private undoStack As Stack(Of IUndoRedoOperation) = New Stack(Of IUndoRedoOperation)()
        Private redoStack As Stack(Of IUndoRedoOperation) = New Stack(Of IUndoRedoOperation)()

        Public Sub New(inkCanvas As InkCanvas, strokeService As InkStrokesService)
            _strokeService = strokeService
            AddHandler _strokeService.StrokesCollected, AddressOf StrokeService_StrokesCollected
            AddHandler _strokeService.StrokesErased, AddressOf StrokeService_StrokesErased
            AddHandler _strokeService.MoveStrokesEvent, AddressOf StrokeService_MoveStrokesEvent
            AddHandler _strokeService.CutStrokesEvent, AddressOf StrokeService_CutStrokesEvent
            AddHandler _strokeService.PasteStrokesEvent, AddressOf StrokeService_PasteStrokesEvent
        End Sub

        Public Event UndoEvent As EventHandler(Of EventArgs)

        Public Event RedoEvent As EventHandler(Of EventArgs)

        Public Event AddUndoOperationEvent As EventHandler(Of EventArgs)

        Public Sub Reset()
            undoStack.Clear()
            redoStack.Clear()
            RaiseEvent AddUndoOperationEvent(Me, EventArgs.Empty)
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
            RaiseEvent UndoEvent(Me, EventArgs.Empty)
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
            RaiseEvent RedoEvent(Me, EventArgs.Empty)
        End Sub

        Public Sub AddOperation(operation As IUndoRedoOperation)
            If operation Is Nothing Then
                Return
            End If

            undoStack.Push(operation)
            redoStack.Clear()
            RaiseEvent AddUndoOperationEvent(Me, EventArgs.Empty)
        End Sub

        Private Sub StrokeService_StrokesCollected(sender As Object, e As InkStrokesCollectedEventArgs)
            AddStrokesOperation(e.Strokes)
        End Sub

        Private Sub StrokeService_StrokesErased(sender As Object, e As InkStrokesErasedEventArgs)
            RemoveStrokesOperation(e.Strokes)
        End Sub

        Private Sub StrokeService_CutStrokesEvent(sender As Object, e As CopyPasteStrokesEventArgs)
           RemoveStrokesOperation(e.Strokes)
        End Sub

        Private Sub StrokeService_PasteStrokesEvent(sender As Object, e As CopyPasteStrokesEventArgs)
            AddStrokesOperation(e.Strokes)
        End Sub

        Private Sub StrokeService_MoveStrokesEvent(sender As Object, e As MoveStrokesEventArgs)
            Dim operation = New MoveStrokesUndoRedoOperation(e.Strokes, e.StartPosition, e.EndPosition, _strokeService)
            AddOperation(operation)
        End Sub

        Private Sub AddStrokesOperation(strokes As IEnumerable(Of InkStroke))
            Dim operation = New AddStrokeUndoRedoOperation(strokes, _strokeService)
            AddOperation(operation)
        End Sub

        Private Sub RemoveStrokesOperation(strokes As IEnumerable(Of InkStroke))
            Dim operation = New RemoveStrokeUndoRedoOperation(strokes, _strokeService)
            AddOperation(operation)
        End Sub
    End Class
End Namespace
