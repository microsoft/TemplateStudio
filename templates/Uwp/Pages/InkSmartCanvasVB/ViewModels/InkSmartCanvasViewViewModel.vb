Imports System.Windows.Input
Imports Param_ItemNamespace.Services.Ink
Imports Param_ItemNamespace.Services.Ink.UndoRedo

Namespace ViewModels
    Public Class InkSmartCanvasViewViewModel
        Inherits System.ComponentModel.INotifyPropertyChanged

        Private _strokeService As InkStrokesService
        Private _lassoSelectionService As InkLassoSelectionService
        Private _nodeSelectionService As InkNodeSelectionService
        Private _pointerDeviceService As InkPointerDeviceService
        Private _undoRedoService As InkUndoRedoService
        Private _transformService As InkTransformService
        Private _fileService As InkFileService
        Private _undoCommand As ICommand
        Private _redoCommand As ICommand
        Private _loadInkFileCommand As ICommand
        Private _saveInkFileCommand As ICommand
        Private _transformTextAndShapesCommand As ICommand
        Private _clearAllCommand As ICommand
        Private _enableTouch As Boolean = True
        Private _enableMouse As Boolean = True
        Private _enablePen As Boolean = True
        Private _enableLassoSelection As Boolean

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
            AddHandler _pointerDeviceService.DetectPenEvent, Sub(s, e) EnableTouch = False
        End Sub

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

        Public ReadOnly Property LoadInkFileCommand As ICommand
            Get
                If _loadInkFileCommand Is Nothing Then
                    _loadInkFileCommand = New RelayCommand(Async Sub()
                                                               ClearSelection()
                                                               Dim fileLoaded = Await _fileService?.LoadInkAsync()

                                                               If fileLoaded Then
                                                                   _transformService.ClearTextAndShapes()
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

        Public ReadOnly Property TransformTextAndShapesCommand As ICommand
            Get
                If _transformTextAndShapesCommand Is Nothing Then
                    _transformTextAndShapesCommand = New RelayCommand(Async Sub()
                                                                          Dim result = Await _transformService.TransformTextAndShapesAsync()

                                                                          If result.TextAndShapes.Any() Then
                                                                              ClearSelection()
                                                                              _undoRedoService.AddOperation(New TransformUndoRedoOperation(result, _strokeService))
                                                                          End If
                                                                      End Sub)
                End If

                Return _transformTextAndShapesCommand
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

        Public Property EnablePen As Boolean
            Get
                Return _enablePen
            End Get
            Set(value As Boolean)
                [Param_Setter](_enablePen, value)
                _pointerDeviceService.EnablePen = value
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
    End Class
End Namespace
