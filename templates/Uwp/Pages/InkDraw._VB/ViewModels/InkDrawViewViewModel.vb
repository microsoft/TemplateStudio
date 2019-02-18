Imports System.Linq
Imports System.Windows.Input
Imports Param_RootNamespace.Services.Ink
Imports Param_RootNamespace.Services.Ink.UndoRedo
Imports Param_RootNamespace.Helpers

Namespace ViewModels
    Public Class InkDrawViewViewModel
        Inherits System.ComponentModel.INotifyPropertyChanged

        Private _strokeService As InkStrokesService
        Private _lassoSelectionService As InkLassoSelectionService
        Private _pointerDeviceService As InkPointerDeviceService
        Private _copyPasteService As InkCopyPasteService
        Private _undoRedoService As InkUndoRedoService
        Private _fileService As InkFileService
        Private _zoomService As InkZoomService
        Private _cutCommand As ICommand
        Private _copyCommand As ICommand
        Private _pasteCommand As ICommand
        Private _undoCommand As ICommand
        Private _redoCommand As ICommand
        Private _zoomInCommand As ICommand
        Private _zoomOutCommand As ICommand
        Private _loadInkFileCommand As ICommand
        Private _saveInkFileCommand As ICommand
        Private _exportAsImageCommand As ICommand
        Private _clearAllCommand As ICommand
        Private _enableTouch As Boolean = True
        Private _enableMouse As Boolean = True
        Private _enableLassoSelection As Boolean

        Public Sub New()
        End Sub

        Public Sub Initialize(strokeService As InkStrokesService, lassoSelectionService As InkLassoSelectionService, pointerDeviceService As InkPointerDeviceService, copyPasteService As InkCopyPasteService, undoRedoService As InkUndoRedoService, fileService As InkFileService, zoomService As InkZoomService)
            _strokeService = strokeService
            _lassoSelectionService = lassoSelectionService
            _pointerDeviceService = pointerDeviceService
            _copyPasteService = copyPasteService
            _undoRedoService = undoRedoService
            _fileService = fileService
            _zoomService = zoomService
            AddHandler _strokeService.CopyStrokesEvent, Sub(s, e) RefreshCommands()
            AddHandler _strokeService.SelectStrokesEvent, Sub(s, e) RefreshCommands()
            AddHandler _strokeService.ClearStrokesEvent, Sub(s, e) RefreshCommands()
            AddHandler _undoRedoService.UndoEvent, Sub(s, e) RefreshCommands()
            AddHandler _undoRedoService.RedoEvent, Sub(s, e) RefreshCommands()
            AddHandler _undoRedoService.AddUndoOperationEvent, Sub(s, e) RefreshCommands()
            AddHandler _pointerDeviceService.DetectPenEvent, Sub(s, e) EnableTouch = False
        End Sub

        Public Property EnableTouch As Boolean
            Get
                Return _enableTouch
            End Get
            Set(value As Boolean)
                [Param_Setter](_enableTouch, value)
                _pointerDeviceService.EnableTouch = value
            End Set
        End Property

        Public Property EnableMouse As Boolean
            Get
                Return _enableMouse
            End Get
            Set(value As Boolean)
                [Param_Setter](_enableMouse, value)
                _pointerDeviceService.EnableMouse = value
            End Set
        End Property

        Public Property EnableLassoSelection As Boolean
            Get
                Return _enableLassoSelection
            End Get
            Set(value As Boolean)
                [Param_Setter](_enableLassoSelection, value)
                ConfigLassoSelection(value)
            End Set
        End Property

        Public ReadOnly Property CutCommand As ICommand
            Get
                If _cutCommand Is Nothing Then
                    _cutCommand = New RelayCommand(AddressOf Cut, AddressOf CanCut)
                End If

                Return _cutCommand
            End Get
        End Property

        Public ReadOnly Property CopyCommand As ICommand
            Get
                If _copyCommand Is Nothing
                    _copyCommand = New RelayCommand(AddressOf Copy, AddressOf CanCopy)

                End If

                Return _copyCommand
            End Get
        End Property

        Public ReadOnly Property PasteCommand As ICommand
            Get
                If _pasteCommand Is Nothing Then
                    _pasteCommand = New RelayCommand(AddressOf Paste, AddressOf CanPaste)
                End If

                Return _pasteCommand
            End Get
        End Property

        Public ReadOnly Property UndoCommand As ICommand
            Get
                If _undoCommand Is Nothing Then
                    _undoCommand = New RelayCommand(AddressOf Undo, AddressOf CanUndo)
                End If

                Return _undoCommand
            End Get
        End Property

        Public ReadOnly Property RedoCommand As ICommand
            Get
                If _redoCommand Is Nothing Then
                    _redoCommand = New RelayCommand(AddressOf Redo, AddressOf CanRedo)
                End If

                Return _redoCommand
            End Get
        End Property

        Public ReadOnly Property ZoomInCommand As ICommand
            Get
                If _zoomInCommand Is Nothing Then
                    _zoomInCommand = New RelayCommand(Sub() _zoomService?.ZoomIn())
                End If

                Return _zoomInCommand
            End Get
        End Property

        Public ReadOnly Property ZoomOutCommand As ICommand
            Get
                If _zoomOutCommand Is Nothing Then
                    _zoomOutCommand = New RelayCommand(Sub() _zoomService?.ZoomOut())
                End If

                Return _zoomOutCommand
            End Get
        End Property

        Public ReadOnly Property LoadInkFileCommand As ICommand
            Get
                If _loadInkFileCommand Is Nothing Then
                    _loadInkFileCommand = New RelayCommand(Async Sub() Await LoadInkFileAsync())
                End If

                Return _loadInkFileCommand
            End Get
        End Property

        Public ReadOnly Property SaveInkFileCommand As ICommand
            Get
                If _saveInkFileCommand Is Nothing Then
                    _saveInkFileCommand = New RelayCommand(Async Sub() Await SaveInkFileAsync(), AddressOf CanSaveInkFile)
                End If

                Return _saveInkFileCommand
            End Get
        End Property

        Public ReadOnly Property ExportAsImageCommand As ICommand
            Get
                If _exportAsImageCommand Is Nothing Then
                    _exportAsImageCommand = New RelayCommand(Async Sub() Await ExportAsImageAsync(), AddressOf CanExportAsImage)
                End If

                Return _exportAsImageCommand
            End Get
        End Property

        Public ReadOnly Property ClearAllCommand As ICommand
            Get
                If _clearAllCommand Is Nothing Then
                    _clearAllCommand = New RelayCommand(AddressOf ClearAll, AddressOf CanClearAll)
                End If

                Return _clearAllCommand
            End Get
        End Property

        Private Sub Cut()
            _copyPasteService?.Cut()
            ClearSelection()
        End Sub

        Private Sub Copy()
            _copyPasteService?.Copy()
        End Sub

        Private Sub Paste()
            _copyPasteService?.Paste()
            ClearSelection()
        End Sub

        Private Sub Undo()
            ClearSelection()
            _undoRedoService?.Undo()
        End Sub

        Private Sub Redo()
            ClearSelection()
            _undoRedoService?.Redo()
        End Sub

        Private Async Function LoadInkFileAsync() as Task
            ClearSelection()
            Dim fileLoaded = Await _fileService?.LoadInkAsync()

            If fileLoaded Then
                _undoRedoService?.Reset()
            End If
        End Function

        Private Async Function SaveInkFileAsync() as Task
            ClearSelection()
            Await _fileService?.SaveInkAsync()
        End Function

        Private Async Function ExportAsImageAsync() as Task
            ClearSelection()
            Await _fileService?.ExportToImageAsync()
        End Function

        Private Sub ClearAll()
            Dim strokes = _strokeService?.GetStrokes().ToList()
            ClearSelection()
            _strokeService?.ClearStrokes()
            _undoRedoService?.AddOperation(new RemoveStrokeUndoRedoOperation(strokes, _strokeService))
        End Sub

        Private Function CanCut() As Boolean
            Return _copyPasteService IsNot Nothing AndAlso _copyPasteService.CanCut
        End Function

        Private Function CanCopy() As Boolean
            Return _copyPasteService IsNot Nothing AndAlso _copyPasteService.CanCopy
        End Function

        Private Function CanPaste() As Boolean
            Return _copyPasteService IsNot Nothing AndAlso _copyPasteService.CanPaste
        End Function

        Private Function CanUndo() As Boolean
            Return _undoRedoService IsNot Nothing AndAlso _undoRedoService.CanUndo
        End Function

        Private Function CanRedo() As Boolean
            Return _undoRedoService IsNot Nothing AndAlso _undoRedoService.CanRedo
        End Function

        Private Function CanSaveInkFile() As Boolean
            Return _strokeService IsNot Nothing AndAlso _strokeService.GetStrokes().Any()
        End Function

        Private Function CanExportAsImage() As Boolean
            Return _strokeService IsNot Nothing AndAlso _strokeService.GetStrokes().Any()
        End Function

        Private Function CanClearAll() As Boolean
            Return _strokeService IsNot Nothing AndAlso _strokeService.GetStrokes().Any()
        End Function

        Private Sub RefreshCommands()
        End Sub

        Private Sub ConfigLassoSelection(enableLasso As Boolean)
            If enableLasso Then
                _lassoSelectionService?.StartLassoSelectionConfig()
            Else
                _lassoSelectionService?.EndLassoSelectionConfig()
            End If
        End Sub

        Private Sub ClearSelection()
            _lassoSelectionService?.ClearSelection()
        End Sub
    End Class
End Namespace
