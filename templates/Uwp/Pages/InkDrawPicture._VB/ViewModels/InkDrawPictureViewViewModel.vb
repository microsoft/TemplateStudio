Imports System.Linq
Imports Param_RootNamespace.Services.Ink
Imports Param_RootNamespace.Helpers
Imports Windows.Storage

Namespace ViewModels
    Public Class InkDrawPictureViewViewModel
        Inherits System.ComponentModel.INotifyPropertyChanged

        Private _strokeService As InkStrokesService
        Private _pointerDeviceService As InkPointerDeviceService
        Private _fileService As InkFileService
        Private _zoomService As InkZoomService
        Private _enableTouch As Boolean = True
        Private _enableMouse As Boolean = True
        Private _image As BitmapImage
        Private _loadImageCommand As ICommand
        Private _saveImageCommand As ICommand
        Private _clearAllCommand As ICommand
        Private _zoomInCommand As ICommand
        Private _zoomOutCommand As ICommand
        Private _resetZoomCommand As ICommand
        Private _fitToScreenCommand As ICommand

        Public Sub New()
        End Sub

        Public Sub Initialize(strokeService As InkStrokesService, pointerDeviceService As InkPointerDeviceService, fileService As InkFileService, zoomService As InkZoomService)
            _strokeService = strokeService
            _pointerDeviceService = pointerDeviceService
            _fileService = fileService
            _zoomService = zoomService
            AddHandler _strokeService.StrokesCollected, Sub(s, e) RefreshCommands()
            AddHandler _strokeService.StrokesErased, Sub(s, e) RefreshCommands()
            AddHandler _strokeService.ClearStrokesEvent, Sub(s, e) RefreshCommands()
            AddHandler _pointerDeviceService.DetectPenEvent, Sub(s, e) EnableTouch = False
        End Sub

        Public Property EnableTouch As Boolean
            Get
                Return _enableTouch
            End Get
            Set(value As Boolean)
                [Param_Setter](_enableTouch, value)
                _pointerDeviceService.EnableTouch = value
            End Set
        End Property

        Public Property EnableMouse As Boolean
            Get
                Return _enableMouse
            End Get
            Set(value As Boolean)
                [Param_Setter](_enableMouse, value)
                _pointerDeviceService.EnableMouse = value
            End Set
        End Property

        Public Property ImageFile As StorageFile

        Public Property Image As BitmapImage
            Get
                Return _image
            End Get
            Set(value As BitmapImage)
                [Param_Setter](_image, value)
                RefreshCommands()
            End Set
        End Property

        Public ReadOnly Property LoadImageCommand As ICommand
            Get
                If _loadImageCommand Is Nothing Then
                    _loadImageCommand = New RelayCommand(Async Sub() Await OnLoadImageAsync())
                End If

                Return _loadImageCommand
            End Get
        End Property

        Public ReadOnly Property SaveImageCommand As ICommand
            Get
                If _saveImageCommand Is Nothing then
                    _saveImageCommand = New RelayCommand(Async Sub() Await OnSaveImageAsync(), AddressOf CanSaveImage)
                End If

                Return _saveImageCommand
            End Get
        End Property

        Public ReadOnly Property ZoomInCommand As ICommand
            Get
                If _zoomInCommand Is Nothing Then
                    _zoomInCommand = New RelayCommand(Sub() _zoomService?.ZoomIn())
                End If

                Return _zoomInCommand
            End Get
        End Property

        Public ReadOnly Property ZoomOutCommand As ICommand
            Get
                If _zoomOutCommand Is Nothing Then
                    _zoomOutCommand = New RelayCommand(Sub() _zoomService?.ZoomOut())
                End If

                Return _zoomOutCommand
            End Get
        End Property

        Public ReadOnly Property ResetZoomCommand As ICommand
            Get
                If _resetZoomCommand Is Nothing Then
                    _resetZoomCommand = New RelayCommand(Sub() _zoomService?.ResetZoom())
                End If

                Return _resetZoomCommand
            End Get
        End Property

        Public ReadOnly Property FitToScreenCommand As ICommand
            Get
                If _fitToScreenCommand Is Nothing Then
                    _fitToScreenCommand = New RelayCommand(Sub() _zoomService?.FitToScreen())
                End If

                Return _fitToScreenCommand
            End Get
        End Property

        Public ReadOnly Property ClearAllCommand As ICommand
            Get
                If _clearAllCommand Is Nothing Then
                    _clearAllCommand = New RelayCommand(AddressOf ClearAll, AddressOf CanClearAll)
                End If

                Return _clearAllCommand
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

        Private Function CanSaveImage() As Boolean
            Return Image IsNot Nothing AndAlso _strokeService IsNot Nothing AndAlso _strokeService.GetStrokes().Any()
        End Function

        Private Function CanClearAll() As Boolean
            Return Image IsNot Nothing OrElse (_strokeService IsNot Nothing AndAlso _strokeService.GetStrokes().Any())
        End Function

        Private Sub ClearAll()
            _strokeService?.ClearStrokes()
            ImageFile = Nothing
            Image = Nothing
        End Sub

        Private Sub RefreshCommands()
        End Sub

    End Class
End Namespace
