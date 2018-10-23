Imports Param_ItemNamespace.Services.Ink
Imports Param_ItemNamespace.Behaviors
Imports Param_ItemNamespace.ViewModels
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls

Namespace Views
    ' For more information regarding Windows Ink documentation and samples see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/ink.md
    Public NotInheritable Partial Class InkDrawPictureViewPage
        Inherits Page

        Public Sub New()
            InitializeComponent()
            SetNavigationViewHeader()
            AddHandler Loaded, Sub(sender, eventArgs)
                                    SetCanvasSize()
                                    AddHandler image.SizeChanged,  AddressOf Image_SizeChanged
                                    Dim strokeService = New InkStrokesService(inkCanvas.InkPresenter)
                                    ViewModel.Initialize(strokeService, New InkPointerDeviceService(inkCanvas), New InkFileService(inkCanvas, strokeService), New InkZoomService(canvasScroll))
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

        Private Sub Image_SizeChanged(sender As Object, e As SizeChangedEventArgs)
            If e.NewSize.Height = 0 OrElse e.NewSize.Width = 0 Then
                SetCanvasSize()
            Else
                inkCanvas.Width = e.NewSize.Width
                inkCanvas.Height = e.NewSize.Height
            End If
        End Sub
    End Class
End Namespace
