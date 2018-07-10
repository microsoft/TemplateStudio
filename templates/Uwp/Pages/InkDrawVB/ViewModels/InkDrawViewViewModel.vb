Imports Param_ItemNamespace.Services.Ink
Imports Param_ItemNamespace.Helpers

Namespace ViewModels
    Public Class InkDrawViewViewModel
        Implements System.ComponentModel.INotifyPropertyChanged

        Private _strokeService As InkStrokesService
        Private _lassoSelectionService As InkLassoSelectionService
        Private _pointerDeviceService As InkPointerDeviceService
        Private _copyPasteService As InkCopyPasteService
        Private _undoRedoService As InkUndoRedoService
        Private _fileService As InkFileService
        Private _zoomService As InkZoomService
        Private cutCommand As RelayCommand
        Private copyCommand As RelayCommand
        Private pasteCommand As RelayCommand
        Private undoCommand As RelayCommand
        Private redoCommand As RelayCommand
        Private zoomInCommand As RelayCommand
        Private zoomOutCommand As RelayCommand
        Private loadInkFileCommand As RelayCommand
        Private saveInkFileCommand As RelayCommand
        Private exportAsImageCommand As RelayCommand
        Private clearAllCommand As RelayCommand
        Private enableTouch As Boolean = True
        Private enableMouse As Boolean = True
        Private enableLassoSelection As Boolean

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
            _pointerDeviceService.DetectPenEvent += Function(s, e) CSharpImpl.__Assign(EnableTouch, False)
        End Sub

        Public ReadOnly Property CutCommand As RelayCommand
            Get
                Return If(cutCommand, (CSharpImpl.__Assign(cutCommand, New RelayCommand(Function()
                                                                                            _copyPasteService?.Cut()
                                                                                            ClearSelection()
                                                                                        End Function))))
            End Get
        End Property

        Public ReadOnly Property CopyCommand As RelayCommand
            Get
                Return If(copyCommand, (CSharpImpl.__Assign(copyCommand, New RelayCommand(Function() _copyPasteService?.Copy()))))
            End Get
        End Property

        Public ReadOnly Property PasteCommand As RelayCommand
            Get
                Return If(pasteCommand, (CSharpImpl.__Assign(pasteCommand, New RelayCommand(Function()
                                                                                                _copyPasteService?.Paste()
                                                                                                ClearSelection()
                                                                                            End Function))))
            End Get
        End Property

        Public ReadOnly Property UndoCommand As RelayCommand
            Get
                Return If(undoCommand, (CSharpImpl.__Assign(undoCommand, New RelayCommand(Function()
                                                                                              ClearSelection()
                                                                                              _undoRedoService?.Undo()
                                                                                          End Function))))
            End Get
        End Property

        Public ReadOnly Property RedoCommand As RelayCommand
            Get
                Return If(redoCommand, (CSharpImpl.__Assign(redoCommand, New RelayCommand(Function()
                                                                                              ClearSelection()
                                                                                              _undoRedoService?.Redo()
                                                                                          End Function))))
            End Get
        End Property

        Public ReadOnly Property ZoomInCommand As RelayCommand
            Get
                Return If(zoomInCommand, (CSharpImpl.__Assign(zoomInCommand, New RelayCommand(Function() _zoomService?.ZoomIn()))))
            End Get
        End Property

        Public ReadOnly Property ZoomOutCommand As RelayCommand
            Get
                Return If(zoomOutCommand, (CSharpImpl.__Assign(zoomOutCommand, New RelayCommand(Function() _zoomService?.ZoomOut()))))
            End Get
        End Property

        Public ReadOnly Property LoadInkFileCommand As RelayCommand
            Get
                Return If(loadInkFileCommand, (CSharpImpl.__Assign(loadInkFileCommand, New RelayCommand(Async Function()
                                                                                                            ClearSelection()
                                                                                                            Dim fileLoaded = Await _fileService?.LoadInkAsync()

                                                                                                            If fileLoaded Then
                                                                                                                _undoRedoService?.Reset()
                                                                                                            End If
                                                                                                        End Function))))
            End Get
        End Property

        Public ReadOnly Property SaveInkFileCommand As RelayCommand
            Get
                Return If(saveInkFileCommand, (CSharpImpl.__Assign(saveInkFileCommand, New RelayCommand(Async Function()
                                                                                                            ClearSelection()
                                                                                                            Await _fileService?.SaveInkAsync()
                                                                                                        End Function))))
            End Get
        End Property

        Public ReadOnly Property ExportAsImageCommand As RelayCommand
            Get
                Return If(exportAsImageCommand, (CSharpImpl.__Assign(exportAsImageCommand, New RelayCommand(Async Function()
                                                                                                                ClearSelection()
                                                                                                                Await _fileService?.ExportToImageAsync()
                                                                                                            End Function))))
            End Get
        End Property

        Public ReadOnly Property ClearAllCommand As RelayCommand
            Get
                Return If(clearAllCommand, (CSharpImpl.__Assign(clearAllCommand, New RelayCommand(AddressOf ClearAll))))
            End Get
        End Property

        Public Property EnableTouch As Boolean
            Get
                Return enableTouch
            End Get
            Set(value As Boolean)
                Param_Setter(enableTouch, value)
                _pointerDeviceService.EnableTouch = value
            End Set
        End Property

        Public Property EnableMouse As Boolean
            Get
                Return enableMouse
            End Get
            Set(value As Boolean)
                Param_Setter(enableMouse, value)
                _pointerDeviceService.EnableMouse = value
            End Set
        End Property

        Public Property EnableLassoSelection As Boolean
            Get
                Return enableLassoSelection
            End Get
            Set(value As Boolean)
                Param_Setter(enableLassoSelection, value)
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
            Return _lassoSelectionService?.ClearSelection()
        End Sub

        Private Class CSharpImpl
            <Obsolete("Please refactor calling code to use normal Visual Basic assignment")>
            Shared Function __Assign(Of T)(ByRef target As T, value As T) As T
                target = value
                Return value
            End Function
        End Class
    End Class
End Namespace
