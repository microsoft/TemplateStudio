Imports Param_ItemNamespace.Services.Ink
Imports Param_ItemNamespace.Helpers
Imports System.Threading.Tasks
Imports System.Windows.Input
Imports Windows.Storage
Imports Windows.UI.Xaml.Media.Imaging

Namespace ViewModels
    ' For more information regarding Windows Ink documentation and samples see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/ink.md
    Public Class InkDrawPictureViewViewModel
        Implements System.ComponentModel.INotifyPropertyChanged

        Private _strokeService As InkStrokesService
        Private _pointerDeviceService As InkPointerDeviceService
        Private _fileService As InkFileService
        Private _zoomService As InkZoomService
        Private enableTouch As Boolean = True
        Private enableMouse As Boolean = True
        Private image As BitmapImage
        Private loadImageCommand As ICommand
        Private saveImageCommand As ICommand
        Private clearAllCommand As ICommand
        Private zoomInCommand As ICommand
        Private zoomOutCommand As ICommand
        Private resetZoomCommand As ICommand
        Private fitToScreenCommand As ICommand

        Public Sub New()
        End Sub

        Public Sub Initialize(strokeService As InkStrokesService, pointerDeviceService As InkPointerDeviceService, fileService As InkFileService, zoomService As InkZoomService)
            _strokeService = strokeService
            _pointerDeviceService = pointerDeviceService
            _fileService = fileService
            _zoomService = zoomService
            _pointerDeviceService.DetectPenEvent += Function(s, e) CSharpImpl.__Assign(EnableTouch, False)
        End Sub

        Public Property EnableTouch As Boolean
            Get
                Return enableTouch
            End Get
            Set(value As Boolean)
                Param_Setter(enableTouch, value)
                _pointerDeviceService.EnableTouch = value
            End Set
        End Property

        Public Property EnableMouse As Boolean
            Get
                Return enableMouse
            End Get
            Set(value As Boolean)
                Param_Setter(enableMouse, value)
                _pointerDeviceService.EnableMouse = value
            End Set
        End Property

        Public Property ImageFile As StorageFile

        Public Property Image As BitmapImage
            Get
                Return image
            End Get
            Set(value As BitmapImage)
                Return Param_Setter(image, value)
            End Set
        End Property

        Public ReadOnly Property LoadImageCommand As ICommand
            Get
                Return If(loadImageCommand, (CSharpImpl.__Assign(loadImageCommand, New RelayCommand(Async Function() Await OnLoadImageAsync()))))
            End Get
        End Property

        Public ReadOnly Property SaveImageCommand As ICommand
            Get
                Return If(saveImageCommand, (CSharpImpl.__Assign(saveImageCommand, New RelayCommand(Async Function() Await OnSaveImageAsync()))))
            End Get
        End Property

        Public ReadOnly Property ZoomInCommand As ICommand
            Get
                Return If(zoomInCommand, (CSharpImpl.__Assign(zoomInCommand, New RelayCommand(Function() _zoomService?.ZoomIn()))))
            End Get
        End Property

        Public ReadOnly Property ZoomOutCommand As ICommand
            Get
                Return If(zoomOutCommand, (CSharpImpl.__Assign(zoomOutCommand, New RelayCommand(Function() _zoomService?.ZoomOut()))))
            End Get
        End Property

        Public ReadOnly Property ResetZoomCommand As ICommand
            Get
                Return If(resetZoomCommand, (CSharpImpl.__Assign(resetZoomCommand, New RelayCommand(Function() _zoomService?.ResetZoom()))))
            End Get
        End Property

        Public ReadOnly Property FitToScreenCommand As ICommand
            Get
                Return If(fitToScreenCommand, (CSharpImpl.__Assign(fitToScreenCommand, New RelayCommand(Function() _zoomService?.FitToScreen()))))
            End Get
        End Property

        Public ReadOnly Property ClearAllCommand As ICommand
            Get
                Return If(clearAllCommand, (CSharpImpl.__Assign(clearAllCommand, New RelayCommand(AddressOf ClearAll))))
            End Get
        End Property

        Private Async Function OnLoadImageAsync() As Task
            Dim file = Await ImageHelper.LoadImageFileAsync()
            Dim bitmapImage = Await ImageHelper.GetBitmapFromImageAsync(file)

            If file IsNot Nothing AndAlso bitmapImage IsNot Nothing Then
                ClearAll()
                ImageFile = file
                Image = bitmapImage
                _zoomService?.FitToSize(Image.PixelWidth, Image.PixelHeight)
            End If
        End Function

        Private Async Function OnSaveImageAsync() As Task
            Await _fileService?.ExportToImageAsync(ImageFile)
        End Function

        Private Sub ClearAll()
            _strokeService?.ClearStrokes()
            ImageFile = Nothing
            Image = Nothing
        End Sub

        Private Class CSharpImpl
            <Obsolete("Please refactor calling code to use normal Visual Basic assignment")>
            Shared Function __Assign(Of T)(ByRef target As T, value As T) As T
                target = value
                Return value
            End Function
        End Class
    End Class
End Namespace
