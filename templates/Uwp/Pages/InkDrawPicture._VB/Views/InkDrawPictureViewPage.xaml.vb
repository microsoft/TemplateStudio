Imports Param_RootNamespace.Services.Ink
Imports Param_RootNamespace.ViewModels
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls

Namespace Views
    ' For more information regarding Windows Ink documentation and samples see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/UWP/pages/ink.md
    Public NotInheritable Partial Class InkDrawPictureViewPage
        Inherits Page

        Public Sub New()
            InitializeComponent()
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
