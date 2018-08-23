Imports Param_ItemNamespace.Services.Ink
Imports Param_ItemNamespace.Services.Ink.UndoRedo

Namespace Views
    ' For more information regarding Windows Ink documentation and samples see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/ink.md
    Public NotInheritable Partial Class InkDrawViewPage
        Inherits Page
        Implements System.ComponentModel.INotifyPropertyChanged

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
                                    touchInkingButton.IsChecked = True
                                    mouseInkingButton.IsChecked = True
                                    AddHandler strokeService.CopyStrokesEvent, Sub(s, e) RefreshEnabledButtons()
                                    AddHandler strokeService.SelectStrokesEvent, Sub(s, e) RefreshEnabledButtons()
                                    AddHandler strokeService.ClearStrokesEvent, Sub(s, e) RefreshEnabledButtons()
                                    AddHandler undoRedoService.UndoEvent, Sub(s, e) RefreshEnabledButtons()
                                    AddHandler undoRedoService.RedoEvent, Sub(s, e) RefreshEnabledButtons()
                                    AddHandler undoRedoService.AddUndoOperationEvent, Sub(s, e) RefreshEnabledButtons()
                                    AddHandler pointerDeviceService.DetectPenEvent, Sub(s, e) touchInkingButton.IsChecked = False
                                End Sub
        End Sub

        Private Sub SetCanvasSize()
            inkCanvas.Width = Math.Max(canvasScroll.ViewportWidth, 1000)
            inkCanvas.Height = Math.Max(canvasScroll.ViewportHeight, 1000)
        End Sub

        Private Sub LassoSelection_Checked(sender As Object, e As RoutedEventArgs)
            lassoSelectionService?.StartLassoSelectionConfig()
        End Sub

        Private Sub LassoSelection_Unchecked(sender As Object, e As RoutedEventArgs)
            lassoSelectionService?.EndLassoSelectionConfig()
        End Sub

        Private Sub TouchInking_Checked(sender As Object, e As RoutedEventArgs)
            pointerDeviceService.EnableTouch = True
        End Sub

        Private Sub TouchInking_Unchecked(sender As Object, e As RoutedEventArgs)
            pointerDeviceService.EnableTouch = False
        End Sub

        Private Sub MouseInking_Checked(sender As Object, e As RoutedEventArgs)
            pointerDeviceService.EnableMouse = True
        End Sub

        Private Sub MouseInking_Unchecked(sender As Object, e As RoutedEventArgs)
            pointerDeviceService.EnableMouse = False
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
            undoRedoService?.AddOperation(new RemoveStrokeUndoRedoOperation(strokes, _strokeService))
        End Sub

        Private Sub RefreshEnabledButtons()
            CutButton.IsEnabled = copyPasteService.CanCut
            CopyButton.IsEnabled = copyPasteService.CanCopy
            PasteButton.IsEnabled = copyPasteService.CanPaste
            UndoButton.IsEnabled = undoRedoService.CanUndo
            RedoButton.IsEnabled = undoRedoService.CanRedo
            SaveInkFileButton.IsEnabled = strokeService.GetStrokes().Any()
            ExportAsImageButton.IsEnabled = strokeService.GetStrokes().Any()
            ClearAllButton.IsEnabled = strokeService.GetStrokes().Any()
        End Sub

        Private Sub ClearSelection()
            lassoSelectionService?.ClearSelection()
        End Sub
    End Class
End Namespace
