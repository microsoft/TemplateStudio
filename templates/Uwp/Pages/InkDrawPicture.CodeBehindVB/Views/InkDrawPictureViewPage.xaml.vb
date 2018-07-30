Imports Param_ItemNamespace.Services.Ink
Imports Param_ItemNamespace.Helpers
Imports Windows.Storage
Imports System.Linq

Namespace Views
    ' For more information regarding Windows Ink documentation and samples see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/ink.md
    Public NotInheritable Partial Class InkDrawPictureViewPage
        Inherits Page
        Implements System.ComponentModel.INotifyPropertyChanged

        Private imageFile As StorageFile
        Private strokesService As InkStrokesService
        Private pointerDeviceService As InkPointerDeviceService
        Private fileService As InkFileService
        Private zoomService As InkZoomService

        Public Sub New()
            InitializeComponent()
            AddHandler Loaded, Sub(sender, eventArgs)
                                    SetCanvasSize()
                                    AddHandler image.SizeChanged, AddressOf Image_SizeChanged
                                    strokesService = New InkStrokesService(inkCanvas.InkPresenter)
                                    pointerDeviceService = New InkPointerDeviceService(inkCanvas)
                                    fileService = New InkFileService(inkCanvas, strokesService)
                                    zoomService = New InkZoomService(canvasScroll)
                                    touchInkingButton.IsChecked = True
                                    mouseInkingButton.IsChecked = True
                                    AddHandler strokesService.StrokesCollected, Sub(s, e) RefreshEnabledButtons()
                                    AddHandler strokesService.StrokesErased, Sub(s, e) RefreshEnabledButtons()
                                    AddHandler strokesService.ClearStrokesEvent, Sub(s, e) RefreshEnabledButtons()
                                    AddHandler pointerDeviceService.DetectPenEvent, Sub(s, e) touchInkingButton.IsChecked = False
                                    RefreshEnabledButtons()
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

        Private Sub ResetZoom_Click(sender As Object, e As RoutedEventArgs)
            zoomService?.ResetZoom()
        End Sub

        Private Sub FitToScreen_Click(sender As Object, e As RoutedEventArgs)
            zoomService?.FitToScreen()
        End Sub

        Private Async Sub LoadImage_Click(sender As Object, e As RoutedEventArgs)
            Dim file = Await ImageHelper.LoadImageFileAsync()
            Dim bitmapImage = Await ImageHelper.GetBitmapFromImageAsync(file)

            If file IsNot Nothing AndAlso bitmapImage IsNot Nothing Then
                ClearAll()
                imageFile = file
                image.Source = bitmapImage
                zoomService?.FitToSize(bitmapImage.PixelWidth, bitmapImage.PixelHeight)
                RefreshEnabledButtons()
            End If
        End Sub

        Private Async Sub SaveImage_Click(sender As Object, e As RoutedEventArgs)
            Await fileService?.ExportToImageAsync(imageFile)
        End Sub

        Private Sub ClearAll_Click(sender As Object, e As RoutedEventArgs)
            ClearAll()
        End Sub

        Private Sub ClearAll()
            strokesService?.ClearStrokes()
            imageFile = Nothing
            image.Source = Nothing
            RefreshEnabledButtons()
        End Sub

        Private Sub RefreshEnabledButtons()
            SaveImageButton.IsEnabled = image.Source IsNot Nothing AndAlso strokesService.GetStrokes().Any()
            ClearAllButton.IsEnabled = image.Source IsNot Nothing OrElse strokesService.GetStrokes().Any()
        End Sub
    End Class
End Namespace
