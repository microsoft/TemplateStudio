Imports Param_RootNamespace.Services.Ink
Imports Windows.UI.Xaml.Controls

Namespace Views

    ' For more information regarding Windows Ink documentation and samples see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/UWP/pages/ink.md
    Public NotInheritable Partial Class InkDrawViewPage
        Inherits Page

        Public Sub New()
            InitializeComponent()
            AddHandler Loaded, Sub(sender, eventArgs)
                                    SetCanvasSize()
                                    Dim strokeService = New InkStrokesService(inkCanvas.InkPresenter)
                                    Dim selectionRectangleService = New InkSelectionRectangleService(inkCanvas, selectionCanvas, strokeService)
                                    ViewModel.Initialize(strokeService, New InkLassoSelectionService(inkCanvas, selectionCanvas, strokeService, selectionRectangleService), New InkPointerDeviceService(inkCanvas), New InkCopyPasteService(strokeService), New InkUndoRedoService(inkCanvas, strokeService), New InkFileService(inkCanvas, strokeService), New InkZoomService(canvasScroll))
                                End Sub
        End Sub

        Private Sub SetCanvasSize()
            inkCanvas.Width = Math.Max(canvasScroll.ViewportWidth, 1000)
            inkCanvas.Height = Math.Max(canvasScroll.ViewportHeight, 1000)
        End Sub
    End Class
End Namespace
