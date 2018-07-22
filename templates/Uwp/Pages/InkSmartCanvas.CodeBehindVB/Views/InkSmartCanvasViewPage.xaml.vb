Imports Param_ItemNamespace.Services.Ink
Imports Param_ItemNamespace.Services.Ink.UndoRedo

Namespace Views
    ' For more information regarding Windows Ink documentation and samples see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/ink.md
    Public NotInheritable Partial Class InkSmartCanvasViewPage
        Inherits Page
        Implements System.ComponentModel.INotifyPropertyChanged

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
                                    strokeService = New InkStrokesService(inkCanvas.InkPresenter.StrokeContainer)
                                    Dim analyzer = New InkAsyncAnalyzer(inkCanvas, strokeService)
                                    Dim selectionRectangleService = New InkSelectionRectangleService(inkCanvas, selectionCanvas, strokeService)
                                    lassoSelectionService = New InkLassoSelectionService(inkCanvas, selectionCanvas, strokeService, selectionRectangleService)
                                    nodeSelectionService = New InkNodeSelectionService(inkCanvas, selectionCanvas, analyzer, strokeService, selectionRectangleService)
                                    pointerDeviceService = New InkPointerDeviceService(inkCanvas)
                                    undoRedoService = New InkUndoRedoService(inkCanvas, strokeService)
                                    transformService = New InkTransformService(drawingCanvas, strokeService)
                                    fileService = New InkFileService(inkCanvas, strokeService)
                                    touchInkingButton.IsChecked = True
                                    mouseInkingButton.IsChecked = True
                                    penInkingButton.IsChecked = True
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

        Private Sub PenInking_Checked(sender As Object, e As RoutedEventArgs)
            pointerDeviceService.EnablePen = True
        End Sub

        Private Sub PenInking_Unchecked(sender As Object, e As RoutedEventArgs)
            pointerDeviceService.EnablePen = False
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
            ClearSelection()
            strokeService.ClearStrokes()
            transformService.ClearTextAndShapes()
            undoRedoService.Reset()
        End Sub

        Private Sub ClearSelection()
            nodeSelectionService.ClearSelection()
            lassoSelectionService.ClearSelection()
        End Sub
    End Class
End Namespace
