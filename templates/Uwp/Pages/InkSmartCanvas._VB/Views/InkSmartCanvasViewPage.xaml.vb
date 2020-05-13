Imports Param_RootNamespace.Helpers
Imports Param_RootNamespace.Services.Ink
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls

Namespace Views
    ' For more information regarding Windows Ink documentation and samples see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/UWP/pages/ink.md
    Public NotInheritable Partial Class InkSmartCanvasViewPage
        Inherits Page

        Public Sub New()
            InitializeComponent()
            AddHandler Loaded, Sub(sender, eventArgs)
                                    SetCanvasSize()
                                    Dim strokeService = New InkStrokesService(inkCanvas.InkPresenter)
                                    Dim analyzer = New InkAsyncAnalyzer(inkCanvas, strokeService)
                                    Dim selectionRectangleService = New InkSelectionRectangleService(inkCanvas, selectionCanvas, strokeService)
                                    ViewModel.Initialize(strokeService, New InkLassoSelectionService(inkCanvas, selectionCanvas, strokeService, selectionRectangleService), New InkNodeSelectionService(inkCanvas, selectionCanvas, analyzer, strokeService, selectionRectangleService), New InkPointerDeviceService(inkCanvas), New InkUndoRedoService(inkCanvas, strokeService), New InkTransformService(drawingCanvas, strokeService), New InkFileService(inkCanvas, strokeService))
                                End Sub
        End Sub

        Private Sub SetCanvasSize()
            inkCanvas.Width = Math.Max(canvasScroll.ViewportWidth, 1000)
            inkCanvas.Height = Math.Max(canvasScroll.ViewportHeight, 1000)
        End Sub
    End Class
End Namespace
