'{[{
Imports Param_ItemNamespace.Helpers
'}]}

Namespace Views

    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page

        Public Sub New()
        '{[{
            AddHandler Loaded, Sub(s, e)
                          SetCanvasSize()
                          Dim strokeService = New InkStrokesService(inkCanvas.InkPresenter)
                          Dim analyzer = New InkAsyncAnalyzer(inkCanvas, strokeService)
                          Dim selectionRectangleService = New InkSelectionRectangleService(inkCanvas, selectionCanvas, strokeService)
                          ViewModel.Initialize(strokeService, New InkLassoSelectionService(inkCanvas, selectionCanvas, strokeService, selectionRectangleService), New InkNodeSelectionService(inkCanvas, selectionCanvas, analyzer, strokeService, selectionRectangleService), New InkPointerDeviceService(inkCanvas), New InkUndoRedoService(inkCanvas, strokeService), New InkTransformService(drawingCanvas, strokeService), New InkFileService(inkCanvas, strokeService))
                      End Sub
        '}]}
        End Sub
    End Class
End Namespace
