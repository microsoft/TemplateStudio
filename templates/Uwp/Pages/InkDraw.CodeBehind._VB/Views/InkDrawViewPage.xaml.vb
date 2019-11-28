Imports Param_RootNamespace.Services.Ink
Imports Param_RootNamespace.Services.Ink.UndoRedo

Namespace Views
    ' For more information regarding Windows Ink documentation and samples see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/UWP/pages/ink.md
    Public NotInheritable Partial Class InkDrawViewPage
        Inherits Page
        Implements System.ComponentModel.INotifyPropertyChanged

        Private _lassoSelectionButtonIsChecked As Boolean
        Private _touchInkingButtonIsChecked As Boolean = True
        Private _mouseInkingButtonIsChecked As Boolean = True
        Private _cutButtonIsEnabled As Boolean
        Private _copyButtonIsEnabled As Boolean
        Private _pasteButtonIsEnabled As Boolean
        Private _undoButtonIsEnabled As Boolean
        Private _redoButtonIsEnabled As Boolean
        Private _saveInkFileButtonIsEnabled As Boolean
        Private _exportAsImageButtonIsEnabled As Boolean
        Private _clearAllButtonIsEnabled As Boolean
        Private strokeService As InkStrokesService
        Private lassoSelectionService As InkLassoSelectionService
        Private pointerDeviceService As InkPointerDeviceService
        Private copyPasteService As InkCopyPasteService
        Private undoRedoService As InkUndoRedoService
        Private fileService As InkFileService
        Private zoomService As InkZoomService

        Public Sub New()
            InitializeComponent()
            AddHandler Loaded, Sub(sender, eventArgs)
                                    SetCanvasSize()
                                    strokeService = New InkStrokesService(inkCanvas.InkPresenter)
                                    Dim selectionRectangleService = New InkSelectionRectangleService(inkCanvas, selectionCanvas, strokeService)
                                    lassoSelectionService = New InkLassoSelectionService(inkCanvas, selectionCanvas, strokeService, selectionRectangleService)
                                    pointerDeviceService = New InkPointerDeviceService(inkCanvas)
                                    copyPasteService = New InkCopyPasteService(strokeService)
                                    undoRedoService = New InkUndoRedoService(inkCanvas, strokeService)
                                    fileService = New InkFileService(inkCanvas, strokeService)
                                    zoomService = New InkZoomService(canvasScroll)
                                    AddHandler strokeService.CopyStrokesEvent, Sub(s, e) RefreshEnabledButtons()
                                    AddHandler strokeService.SelectStrokesEvent, Sub(s, e) RefreshEnabledButtons()
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

        Public Property CutButtonIsEnabled As Boolean
            Get
                Return _cutButtonIsEnabled
            End Get
            Set(value As Boolean)
                [Param_Setter](_cutButtonIsEnabled, value)
            End Set
        End Property

        Public Property CopyButtonIsEnabled As Boolean
            Get
                Return _copyButtonIsEnabled
            End Get
            Set(value As Boolean)
                [Param_Setter](_copyButtonIsEnabled, value)
            End Set
        End Property

        Public Property PasteButtonIsEnabled As Boolean
            Get
                Return _pasteButtonIsEnabled
            End Get
            Set(value As Boolean)
                [Param_Setter](_pasteButtonIsEnabled, value)
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

        Public Property ExportAsImageButtonIsEnabled As Boolean
            Get
                Return _exportAsImageButtonIsEnabled
            End Get
            Set(value As Boolean)
                [Param_Setter](_exportAsImageButtonIsEnabled, value)
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

        Private Sub ZoomIn_Click(sender As Object, e As RoutedEventArgs)
            zoomService?.ZoomIn()
        End Sub

        Private Sub ZoomOut_Click(sender As Object, e As RoutedEventArgs)
            zoomService?.ZoomOut()
        End Sub

        Private Sub Copy_Click(sender As Object, e As RoutedEventArgs)
            copyPasteService?.Copy()
        End Sub

        Private Sub Cut_Click(sender As Object, e As RoutedEventArgs)
            copyPasteService?.Cut()
            ClearSelection()
        End Sub

        Private Sub Paste_Click(sender As Object, e As RoutedEventArgs)
            copyPasteService?.Paste()
            ClearSelection()
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
            Dim fileLoaded = Await fileService?.LoadInkAsync()

            If fileLoaded Then
                undoRedoService?.Reset()
            End If
        End Sub

        Private Async Sub SaveInkFile_Click(sender As Object, e As RoutedEventArgs)
            ClearSelection()
            Await fileService?.SaveInkAsync()
        End Sub

        Private Async Sub ExportAsImage_Click(sender As Object, e As RoutedEventArgs)
            ClearSelection()
            Await fileService?.ExportToImageAsync()
        End Sub

        Private Sub ClearAll_Click(sender As Object, e As RoutedEventArgs)
            Dim strokes = strokeService?.GetStrokes().ToList()
            ClearSelection()
            strokeService?.ClearStrokes()
            undoRedoService?.AddOperation(new RemoveStrokeUndoRedoOperation(strokes, strokeService))
        End Sub

        Private Sub RefreshEnabledButtons()
            CutButtonIsEnabled = copyPasteService.CanCut
            CopyButtonIsEnabled = copyPasteService.CanCopy
            PasteButtonIsEnabled = copyPasteService.CanPaste
            UndoButtonIsEnabled = undoRedoService.CanUndo
            RedoButtonIsEnabled = undoRedoService.CanRedo
            SaveInkFileButtonIsEnabled = strokeService.GetStrokes().Any()
            ExportAsImageButtonIsEnabled = strokeService.GetStrokes().Any()
            ClearAllButtonIsEnabled = strokeService.GetStrokes().Any()
        End Sub

        Private Sub ConfigLassoSelection(enableLasso As Boolean)
            If enableLasso Then
                lassoSelectionService?.StartLassoSelectionConfig()
            Else
                lassoSelectionService?.EndLassoSelectionConfig()
            End If
        End Sub

        Private Sub ClearSelection()
            lassoSelectionService?.ClearSelection()
        End Sub
    End Class
End Namespace
