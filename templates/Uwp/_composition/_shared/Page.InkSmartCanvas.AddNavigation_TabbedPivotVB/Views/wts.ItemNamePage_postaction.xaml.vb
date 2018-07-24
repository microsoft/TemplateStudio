'{[{
Imports Param_ItemNamespace.Helpers
'}]}

Namespace Views

    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page

        '{[{
        private viewModelinitialized as Boolean = False
        '}]}

        Public Sub New()
        End Sub

        '{[{
        Public Async Function OnPivotSelectedAsync() As Task Implements IPivotPage.OnPivotSelectedAsync
            if (viewModelinitialized)
                return
            end if

            SetCanvasSize()
            Dim strokeService = New InkStrokesService(inkCanvas.InkPresenter)
            Dim analyzer = New InkAsyncAnalyzer(inkCanvas, strokeService)
            Dim selectionRectangleService = New InkSelectionRectangleService(inkCanvas, selectionCanvas, strokeService)
            ViewModel.Initialize(strokeService, New InkLassoSelectionService(inkCanvas, selectionCanvas, strokeService, selectionRectangleService), New InkNodeSelectionService(inkCanvas, selectionCanvas, analyzer, strokeService, selectionRectangleService), New InkPointerDeviceService(inkCanvas), New InkUndoRedoService(inkCanvas, strokeService), New InkTransformService(drawingCanvas, strokeService), New InkFileService(inkCanvas, strokeService))
            viewModelinitialized = True
            Await Task.CompletedTask
        End Function

        Public Async Function OnPivotUnselectedAsync() As Task Implements IPivotPage.OnPivotUnselectedAsync
            Await Task.CompletedTask
        End Function
        '}]}
    End Class
End Namespace
