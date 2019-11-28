Imports Param_RootNamespace.Services.Ink
Imports Param_RootNamespace.Services.Ink.UndoRedo

Namespace Views
    ' For more information regarding Windows Ink documentation and samples see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/UWP/pages/ink.md
    Public NotInheritable Partial Class InkSmartCanvasViewPage
        Inherits Page
        Implements System.ComponentModel.INotifyPropertyChanged

        Private _lassoSelectionButtonIsChecked As Boolean
        Private _touchInkingButtonIsChecked As Boolean = True
        Private _mouseInkingButtonIsChecked As Boolean = True
        Private _penInkingButtonIsChecked As Boolean = True
        Private _transformTextAndShapesButtonIsEnabled As Boolean
        Private _undoButtonIsEnabled As Boolean
        Private _redoButtonIsEnabled As Boolean
        Private _saveInkFileButtonIsEnabled As Boolean
        Private _clearAllButtonIsEnabled As Boolean
        Private strokeService As InkStrokesService
        Private lassoSelectionService As InkLassoSelectionService
        Private nodeSelectionService As InkNodeSelectionService
        Private pointerDeviceService As InkPointerDeviceService
        Private undoRedoService As InkUndoRedoService
        Private transformService As InkTransformService
        Private fileService As InkFileService

        Public Sub New()
            InitializeComponent()
            AddHandler Loaded, Sub(sender, eventArgs)
                                    SetCanvasSize()
                                    strokeService = New InkStrokesService(inkCanvas.InkPresenter)
                                    Dim analyzer = New InkAsyncAnalyzer(inkCanvas, strokeService)
                                    Dim selectionRectangleService = New InkSelectionRectangleService(inkCanvas, selectionCanvas, strokeService)
                                    lassoSelectionService = New InkLassoSelectionService(inkCanvas, selectionCanvas, strokeService, selectionRectangleService)
                                    nodeSelectionService = New InkNodeSelectionService(inkCanvas, selectionCanvas, analyzer, strokeService, selectionRectangleService)
                                    pointerDeviceService = New InkPointerDeviceService(inkCanvas)
                                    undoRedoService = New InkUndoRedoService(inkCanvas, strokeService)
                                    transformService = New InkTransformService(drawingCanvas, strokeService)
                                    fileService = New InkFileService(inkCanvas, strokeService)
                                    AddHandler strokeService.ClearStrokesEvent, Sub(s, e) RefreshEnabledButtons()
                                    AddHandler undoRedoService.UndoEvent, Sub(s, e) RefreshEnabledButtons()
                                    AddHandler undoRedoService.RedoEvent, Sub(s, e) RefreshEnabledButtons()
                                    AddHandler undoRedoService.AddUndoOperationEvent, Sub(s, e) RefreshEnabledButtons()
                                    AddHandler pointerDeviceService.DetectPenEvent, Sub(s, e) TouchInkingButtonIsChecked = False
                                End Sub
        End Sub

        Public Property LassoSelectionButtonIsChecked As Boolean
            Get
                Return _lassoSelectionButtonIsChecked
            End Get
            Set(value As Boolean)
                [Param_Setter](_lassoSelectionButtonIsChecked, value)
                ConfigLassoSelection(value)
            End Set
        End Property

        Public Property TouchInkingButtonIsChecked As Boolean
            Get
                Return _touchInkingButtonIsChecked
            End Get
            Set(value As Boolean)
                [Param_Setter](_touchInkingButtonIsChecked, value)
                pointerDeviceService.EnableTouch = value
            End Set
        End Property

        Public Property MouseInkingButtonIsChecked As Boolean
            Get
                Return _mouseInkingButtonIsChecked
            End Get
            Set(value As Boolean)
                [Param_Setter](_mouseInkingButtonIsChecked, value)
                pointerDeviceService.EnableMouse = value
            End Set
        End Property

        Public Property PenInkingButtonIsChecked As Boolean
            Get
                Return _penInkingButtonIsChecked
            End Get
            Set(value As Boolean)
                [Param_Setter](_penInkingButtonIsChecked, value)
                pointerDeviceService.EnablePen = value
            End Set
        End Property

        Public Property TransformTextAndShapesButtonIsEnabled As Boolean
            Get
                Return _transformTextAndShapesButtonIsEnabled
            End Get
            Set(value As Boolean)
                [Param_Setter](_transformTextAndShapesButtonIsEnabled, value)
            End Set
        End Property

        Public Property UndoButtonIsEnabled As Boolean
            Get
                Return _undoButtonIsEnabled
            End Get
            Set(value As Boolean)
                [Param_Setter](_undoButtonIsEnabled, value)
            End Set
        End Property

        Public Property RedoButtonIsEnabled As Boolean
            Get
                Return _redoButtonIsEnabled
            End Get
            Set(value As Boolean)
                [Param_Setter](_redoButtonIsEnabled, value)
            End Set
        End Property

        Public Property SaveInkFileButtonIsEnabled As Boolean
            Get
                Return _saveInkFileButtonIsEnabled
            End Get
            Set(value As Boolean)
                [Param_Setter](_saveInkFileButtonIsEnabled, value)
            End Set
        End Property

        Public Property ClearAllButtonIsEnabled As Boolean
            Get
                Return _clearAllButtonIsEnabled
            End Get
            Set(value As Boolean)
                [Param_Setter](_clearAllButtonIsEnabled, value)
            End Set
        End Property

        Private Sub SetCanvasSize()
            inkCanvas.Width = Math.Max(canvasScroll.ViewportWidth, 1000)
            inkCanvas.Height = Math.Max(canvasScroll.ViewportHeight, 1000)
        End Sub

        Private Sub Undo_Click(sender As Object, e As RoutedEventArgs)
            ClearSelection()
            undoRedoService?.Undo()
        End Sub

        Private Sub Redo_Click(sender As Object, e As RoutedEventArgs)
            ClearSelection()
            undoRedoService?.Redo()
        End Sub

        Private Async Sub LoadInkFile_Click(sender As Object, e As RoutedEventArgs)
            ClearSelection()
            Dim fileLoaded = Await fileService.LoadInkAsync()

            If fileLoaded Then
                transformService.ClearTextAndShapes()
                undoRedoService.Reset()
            End If
        End Sub

        Private Async Sub SaveInkFile_Click(sender As Object, e As RoutedEventArgs)
            ClearSelection()
            Await fileService.SaveInkAsync()
        End Sub

        Private Async Sub TransformTextAndShapes_Click(sender As Object, e As RoutedEventArgs)
            Dim result = Await transformService.TransformTextAndShapesAsync()

            If result.TextAndShapes.Any() Then
                ClearSelection()
                undoRedoService.AddOperation(New TransformUndoRedoOperation(result, strokeService))
            End If
        End Sub

        Private Sub ClearAll_Click(sender As Object, e As RoutedEventArgs)
            Dim strokes = strokeService?.GetStrokes().ToList()
            Dim textAndShapes = transformService?.GetTextAndShapes().ToList()
            ClearSelection()
            strokeService.ClearStrokes()
            transformService.ClearTextAndShapes()
            undoRedoService?.AddOperation(new ClearStrokesAndShapesUndoRedoOperation(strokes, textAndShapes, strokeService, transformService))
        End Sub

        Private Sub RefreshEnabledButtons()
            UndoButtonIsEnabled = undoRedoService.CanUndo
            RedoButtonIsEnabled = undoRedoService.CanRedo
            SaveInkFileButtonIsEnabled = strokeService.GetStrokes().Any()
            TransformTextAndShapesButtonIsEnabled = strokeService.GetStrokes().Any()
            ClearAllButtonIsEnabled = strokeService.GetStrokes().Any() OrElse transformService.HasTextAndShapes()
        End Sub

        Private Sub ConfigLassoSelection(enableLasso As Boolean)
            If enableLasso Then
                lassoSelectionService?.StartLassoSelectionConfig()
            Else
                lassoSelectionService?.EndLassoSelectionConfig()
            End If
        End Sub

        Private Sub ClearSelection()
            nodeSelectionService.ClearSelection()
            lassoSelectionService.ClearSelection()
        End Sub
    End Class
End Namespace
