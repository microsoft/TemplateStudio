Imports System.Windows.Input
Imports Param_ItemNamespace.Services.Ink
Imports Param_ItemNamespace.Helpers

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
            AddHandler _pointerDeviceService.DetectPenEvent, Sub(s, e) EnableTouch = False
        End Sub

        Public ReadOnly Property CutCommand As ICommand
            Get
                If _cutCommand Is Nothing Then
                    _cutCommand = New RelayCommand(Sub()
                                                       _copyPasteService?.Cut()
                                                       ClearSelection()
                                                   End Sub)
                End If

                Return _cutCommand
            End Get
        End Property

        Public ReadOnly Property CopyCommand As ICommand
            Get
                If _copyCommand Is Nothing
                    _copyCommand = New RelayCommand(Sub() _copyPasteService?.Copy())

                End If

                Return _copyCommand
            End Get
        End Property

        Public ReadOnly Property PasteCommand As ICommand
            Get
                If _pasteCommand Is Nothing Then
                    _pasteCommand = New RelayCommand(Sub()
                                                         _copyPasteService?.Paste()
                                                         ClearSelection()
                                                     End Sub)
                End If

                Return _pasteCommand
            End Get
        End Property

        Public ReadOnly Property UndoCommand As ICommand
            Get
                If _undoCommand Is Nothing Then
                    _undoCommand = New RelayCommand(Sub()
                                                        ClearSelection()
                                                        _undoRedoService?.Undo()
                                                    End Sub)
                End If

                Return _undoCommand
            End Get
        End Property

        Public ReadOnly Property RedoCommand As ICommand
            Get
                If _redoCommand Is Nothing Then
                    _redoCommand = New RelayCommand(Sub()
                                                        ClearSelection()
                                                        _undoRedoService?.Redo()
                                                    End Sub)
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
                    _loadInkFileCommand = New RelayCommand(Async Sub()
                                                               ClearSelection()
                                                               Dim fileLoaded = Await _fileService?.LoadInkAsync()

                                                               If fileLoaded Then
                                                                   _undoRedoService?.Reset()
                                                               End If
                                                           End Sub)
                End If

                Return _loadInkFileCommand
            End Get
        End Property

        Public ReadOnly Property SaveInkFileCommand As ICommand
            Get
                If _saveInkFileCommand Is Nothing Then
                    _saveInkFileCommand = New RelayCommand(Async Sub()
                                                               ClearSelection()
                                                               Await _fileService?.SaveInkAsync()
                                                           End Sub)
                End If

                Return _saveInkFileCommand
            End Get
        End Property

        Public ReadOnly Property ExportAsImageCommand As ICommand
            Get
                If _exportAsImageCommand Is Nothing Then
                    _exportAsImageCommand = New RelayCommand(Async Sub()
                                                                 ClearSelection()
                                                                 Await _fileService?.ExportToImageAsync()
                                                             End Sub)
                End If

                Return _exportAsImageCommand
            End Get
        End Property

        Public ReadOnly Property ClearAllCommand As ICommand
            Get
                If _clearAllCommand Is Nothing Then
                    _clearAllCommand = New RelayCommand(AddressOf ClearAll)
                End If

                Return _clearAllCommand
            End Get
        End Property

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

        Private Sub ConfigLassoSelection(enableLasso As Boolean)
            If enableLasso Then
                _lassoSelectionService?.StartLassoSelectionConfig()
            Else
                _lassoSelectionService?.EndLassoSelectionConfig()
            End If
        End Sub

        Private Sub ClearAll()
            ClearSelection()
            _strokeService?.ClearStrokes()
            _undoRedoService?.Reset()
        End Sub

        Private Sub ClearSelection()
            _lassoSelectionService?.ClearSelection()
        End Sub
    End Class
End Namespace
