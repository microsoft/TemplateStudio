Imports Param_ItemNamespace.Services.Ink
Imports Param_ItemNamespace.Services.Ink.UndoRedo
Imports System.Linq

Namespace ViewModels
    Public Class InkSmartCanvasViewViewModel
        Implements System.ComponentModel.INotifyPropertyChanged

        Private _strokeService As InkStrokesService
        Private _lassoSelectionService As InkLassoSelectionService
        Private _nodeSelectionService As InkNodeSelectionService
        Private _pointerDeviceService As InkPointerDeviceService
        Private _undoRedoService As InkUndoRedoService
        Private _transformService As InkTransformService
        Private _fileService As InkFileService
        Private undoCommand As RelayCommand
        Private redoCommand As RelayCommand
        Private loadInkFileCommand As RelayCommand
        Private saveInkFileCommand As RelayCommand
        Private transformTextAndShapesCommand As RelayCommand
        Private clearAllCommand As RelayCommand
        Private enableTouch As Boolean = True
        Private enableMouse As Boolean = True
        Private enablePen As Boolean = True
        Private enableLassoSelection As Boolean

        Public Sub New()
        End Sub

        Public Sub Initialize(strokeService As InkStrokesService, lassoSelectionService As InkLassoSelectionService, nodeSelectionService As InkNodeSelectionService, pointerDeviceService As InkPointerDeviceService, undoRedoService As InkUndoRedoService, transformService As InkTransformService, fileService As InkFileService)
            _strokeService = strokeService
            _lassoSelectionService = lassoSelectionService
            _nodeSelectionService = nodeSelectionService
            _pointerDeviceService = pointerDeviceService
            _undoRedoService = undoRedoService
            _transformService = transformService
            _fileService = fileService
            _pointerDeviceService.DetectPenEvent += Function(s, e) CSharpImpl.__Assign(EnableTouch, False)
        End Sub

        Public ReadOnly Property UndoCommand As RelayCommand
            Get
                Return If(undoCommand, (CSharpImpl.__Assign(undoCommand, New RelayCommand(Function()
                                                                                              ClearSelection()
                                                                                              _undoRedoService.Undo()
                                                                                          End Function))))
            End Get
        End Property

        Public ReadOnly Property RedoCommand As RelayCommand
            Get
                Return If(redoCommand, (CSharpImpl.__Assign(redoCommand, New RelayCommand(Function()
                                                                                              ClearSelection()
                                                                                              _undoRedoService.Redo()
                                                                                          End Function))))
            End Get
        End Property

        Public ReadOnly Property LoadInkFileCommand As RelayCommand
            Get
                Return If(loadInkFileCommand, (CSharpImpl.__Assign(loadInkFileCommand, New RelayCommand(Async Function()
                                                                                                            ClearSelection()
                                                                                                            Dim fileLoaded = Await _fileService.LoadInkAsync()

                                                                                                            If fileLoaded Then
                                                                                                                _transformService.ClearTextAndShapes()
                                                                                                                _undoRedoService.Reset()
                                                                                                            End If
                                                                                                        End Function))))
            End Get
        End Property

        Public ReadOnly Property SaveInkFileCommand As RelayCommand
            Get
                Return If(saveInkFileCommand, (CSharpImpl.__Assign(saveInkFileCommand, New RelayCommand(Async Function()
                                                                                                            ClearSelection()
                                                                                                            Await _fileService.SaveInkAsync()
                                                                                                        End Function))))
            End Get
        End Property

        Public ReadOnly Property TransformTextAndShapesCommand As RelayCommand
            Get
                Return If(transformTextAndShapesCommand, (CSharpImpl.__Assign(transformTextAndShapesCommand, New RelayCommand(Async Function()
                                                                                                                                  Dim result = Await _transformService.TransformTextAndShapesAsync()

                                                                                                                                  If result.TextAndShapes.Any() Then
                                                                                                                                      ClearSelection()
                                                                                                                                      _undoRedoService.AddOperation(New TransformUndoRedoOperation(result, _strokeService))
                                                                                                                                  End If
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

        Public Property EnablePen As Boolean
            Get
                Return enablePen
            End Get
            Set(value As Boolean)
                Param_Setter(enablePen, value)
                _pointerDeviceService.EnablePen = value
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
                _lassoSelectionService.StartLassoSelectionConfig()
            Else
                _lassoSelectionService.EndLassoSelectionConfig()
            End If
        End Sub

        Private Sub ClearSelection()
            _nodeSelectionService.ClearSelection()
            _lassoSelectionService.ClearSelection()
        End Sub

        Private Sub ClearAll()
            ClearSelection()
            _strokeService.ClearStrokes()
            _transformService.ClearTextAndShapes()
            _undoRedoService.Reset()
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
