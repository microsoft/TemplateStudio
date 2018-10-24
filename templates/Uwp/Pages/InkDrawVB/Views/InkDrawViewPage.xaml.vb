Imports Param_ItemNamespace.Services.Ink
Imports Param_ItemNamespace.Behaviors
Imports Windows.UI.Xaml.Controls

Namespace Views

    ' For more information regarding Windows Ink documentation and samples see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/ink.md
    Public NotInheritable Partial Class InkDrawViewPage
        Inherits Page

        Public Sub New()
            InitializeComponent()
            NavigationViewHeaderBehavior.SetHeaderContext(Me, Me)
            SetNavigationViewHeader()
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

        Private Sub OnInkToolbarLoaded(sender As Object, e As RoutedEventArgs)
            Dim inkToolbar As InkToolbar = TryCast(sender, InkToolbar)
            If inkToolbar IsNot Nothing Then
                inkToolbar.TargetInkCanvas = inkCanvas
            End If
        End Sub

        Private Sub VisualStateGroup_CurrentStateChanged(sender As Object, e As VisualStateChangedEventArgs)
            SetNavigationViewHeader()
        End Sub

        Private Sub SetNavigationViewHeader()
            If visualStateGroup.CurrentState IsNot Nothing Then

                Select Case visualStateGroup.CurrentState.Name
                    Case "BigVisualState"
                        NavigationViewHeaderBehavior.SetHeaderTemplate(Me, TryCast(Resources("BigHeaderTemplate"), DataTemplate))
                        bottomCommandBar.Visibility = Visibility.Collapsed
                    Case "SmallVisualState"
                        NavigationViewHeaderBehavior.SetHeaderTemplate(Me, TryCast(Resources("SmallHeaderTemplate"), DataTemplate))
                        bottomCommandBar.Visibility = Visibility.Visible
                End Select
            End If
        End Sub
    End Class
End Namespace
