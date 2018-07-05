Imports Param_ItemNamespace.Helpers
Imports Param_ItemNamespace.EventHandlers

Imports Windows.Devices.Enumeration
Imports Windows.Devices.Sensors
Imports Windows.Graphics.Imaging
Imports Windows.Media.Capture
Imports Windows.Media.MediaProperties
Imports Windows.Storage
Imports Windows.Storage.FileProperties
Imports Windows.Storage.Streams
Imports Windows.UI.Core

Namespace Controls
    Public NotInheritable Partial Class CameraControl
      Public Event PhotoTaken As EventHandler(Of CameraControlEventArgs)

      Public Shared ReadOnly CanSwitchProperty As DependencyProperty = DependencyProperty.Register("CanSwitch", GetType(Boolean), GetType(CameraControl), New PropertyMetadata(False))

      Public Shared ReadOnly PanelProperty As DependencyProperty = DependencyProperty.Register("Panel", GetType(Panel), GetType(CameraControl), New PropertyMetadata(Panel.Front, AddressOf OnPanelChanged))

      Public Shared ReadOnly IsInitializedProperty As DependencyProperty = DependencyProperty.Register("IsInitialized", GetType(Boolean), GetType(CameraControl), New PropertyMetadata(False))

      Public Shared ReadOnly CameraButtonStyleProperty As DependencyProperty = DependencyProperty.Register("CameraButtonStyle", GetType(Style), GetType(CameraControl), New PropertyMetadata(Nothing))

      Public Shared ReadOnly SwitchCameraButtonStyleProperty As DependencyProperty = DependencyProperty.Register("SwitchCameraButtonStyle", GetType(Style), GetType(CameraControl), New PropertyMetadata(Nothing))

      ' Rotation metadata to apply to the preview stream and recorded videos (MF_MT_VIDEO_ROTATION)
      ' Reference: http://msdn.microsoft.com/en-us/library/windows/apps/xaml/hh868174.aspx
      Private ReadOnly _rotationKey As New Guid("C380465D-2271-428C-9B83-ECEA3B4A85C1")
      Private ReadOnly _displayInformation As DisplayInformation = DisplayInformation.GetForCurrentView()
      Private ReadOnly _orientationSensor As SimpleOrientationSensor = SimpleOrientationSensor.GetDefault()
      Private _mediaCapture As MediaCapture
      Private _isPreviewing As Boolean
      Private _mirroringPreview As Boolean
      Private _deviceOrientation As SimpleOrientation = SimpleOrientation.NotRotated
      Private _displayOrientation As DisplayOrientations = DisplayOrientations.Portrait
      Private _cameraDevices As DeviceInformationCollection
      Private _capturing As Boolean

      Public Property CanSwitch As Boolean
          Get
              Return CBool(GetValue(CanSwitchProperty))
          End Get
          Set
              SetValue(CanSwitchProperty, value)
          End Set
      End Property

      Public Property Panel As Panel
          Get
              Return DirectCast(GetValue(PanelProperty), Panel)
          End Get
          Set
              SetValue(PanelProperty, value)
          End Set
      End Property

      Public Property IsInitialized As Boolean
          Get
              Return CBool(GetValue(IsInitializedProperty))
          End Get
          Private Set
              SetValue(IsInitializedProperty, value)
          End Set
      End Property

      Public Property CameraButtonStyle As Style
          Get
              Return DirectCast(GetValue(CameraButtonStyleProperty), Style)
          End Get
          Set
              SetValue(CameraButtonStyleProperty, value)
          End Set
      End Property

      Public Property SwitchCameraButtonStyle As Style
          Get
              Return DirectCast(GetValue(SwitchCameraButtonStyleProperty), Style)
          End Get
          Set
              SetValue(SwitchCameraButtonStyleProperty, value)
          End Set
      End Property

      Public Sub New()
          InitializeComponent()

          CameraButtonStyle = TryCast(Resources("CameraButtonStyle"), Style)
          SwitchCameraButtonStyle = TryCast(Resources("SwitchCameraButtonStyle"), Style)
      End Sub

      Public Async Function InitializeCameraAsync() As Task
          Try
              If _mediaCapture Is Nothing Then
                  _mediaCapture = New MediaCapture()
                  AddHandler _mediaCapture.Failed, AddressOf MediaCapture_Failed

                  _cameraDevices = Await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture)
                  If _cameraDevices Is Nothing OrElse Not _cameraDevices.Any() Then
                      Throw New NotSupportedException()
                  End If

                  Dim device = _cameraDevices.FirstOrDefault(Function(camera) camera.EnclosureLocation?.Panel = Panel)

                  Dim cameraId = If(device?.Id, _cameraDevices.First().Id)

                  Await _mediaCapture.InitializeAsync(New MediaCaptureInitializationSettings() With {
                      .VideoDeviceId = cameraId
                  })

                  If Panel = Panel.Back Then
                      _mediaCapture.SetRecordRotation(VideoRotation.Clockwise90Degrees)
                      _mediaCapture.SetPreviewRotation(VideoRotation.Clockwise90Degrees)
                      _mirroringPreview = False
                  Else
                      _mirroringPreview = True
                  End If

                  IsInitialized = True
                  CanSwitch = _cameraDevices?.Count > 1
                  RegisterOrientationEventHandlers()
                  Await StartPreviewAsync()
              End If
          Catch ex As UnauthorizedAccessException
              Throw New UnauthorizedAccessException("Camera_Exception_UnauthorizedAccess".GetLocalized(), ex)
          Catch ex As NotSupportedException
                Throw New NotSupportedException("Camera_Exception_NotSupported".GetLocalized(), ex)
          End Try
      End Function

      Public Async Function CleanupCameraAsync() As Task
          If IsInitialized Then
              If _isPreviewing Then
                  Await StopPreviewAsync()
              End If

              UnregisterOrientationEventHandlers()
              IsInitialized = False
          End If

          If _mediaCapture IsNot Nothing Then
              RemoveHandler _mediaCapture.Failed, AddressOf MediaCapture_Failed
              _mediaCapture.Dispose()
              _mediaCapture = Nothing
          End If
      End Function

      Public Async Function TakePhoto() As Task(Of String)
          If _capturing Then
              Return Nothing
          End If

          _capturing = True

          Using stream = New InMemoryRandomAccessStream()
              Await _mediaCapture.CapturePhotoToStreamAsync(ImageEncodingProperties.CreateJpeg(), stream)

              Dim photoOrientation = _displayInformation.ToSimpleOrientation(_deviceOrientation, _mirroringPreview).ToPhotoOrientation(_mirroringPreview)

              Dim photo = Await ReencodeAndSavePhotoAsync(stream, photoOrientation)
              RaiseEvent PhotoTaken(Me, New CameraControlEventArgs(photo))
              _capturing = False
              Return photo
          End Using
      End Function

      Public Sub SwitchPanel()
          Panel = If((Panel = Panel.Front), Panel.Back, Panel.Front)
      End Sub

      Private Async Sub CaptureButton_Click(sender As Object, e As RoutedEventArgs)
          Await TakePhoto()
      End Sub

      Private Sub SwitchButton_Click(sender As Object, e As RoutedEventArgs)
          SwitchPanel()
      End Sub

      Private Async Sub CleanAndInitialize()
          Await Task.Run(Async Function()
                             Await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Async Sub() 
                                    Await CleanupCameraAsync()
                                    Await InitializeCameraAsync()
                                 End Sub)
                         End Function)
      End Sub

      Private Async Sub MediaCapture_Failed(sender As MediaCapture, errorEventArgs As MediaCaptureFailedEventArgs)
        Await Task.Run(Async Function()
                           Await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Async Sub() 
                                    Await CleanupCameraAsync()
                                End Sub)
                    End Function)
      End Sub

      Private Async Function StartPreviewAsync() As Task
          PreviewControl.Source = _mediaCapture
          PreviewControl.FlowDirection = If(_mirroringPreview, FlowDirection.RightToLeft, FlowDirection.LeftToRight)

          If _mediaCapture IsNot Nothing Then
              Await _mediaCapture.StartPreviewAsync()
              Await SetPreviewRotationAsync()
              _isPreviewing = True
          End If
      End Function

      Private Async Function SetPreviewRotationAsync() As Task
          _displayOrientation = _displayInformation.CurrentOrientation
          Dim rotationDegrees As Integer = _displayOrientation.ToDegrees()

          If _mirroringPreview Then
              rotationDegrees = (360 - rotationDegrees) Mod 360
          End If

          If _mediaCapture IsNot Nothing Then
              Dim props = _mediaCapture.VideoDeviceController.GetMediaStreamProperties(MediaStreamType.VideoPreview)
              props.Properties.Add(_rotationKey, rotationDegrees)
              Await _mediaCapture.SetEncodingPropertiesAsync(MediaStreamType.VideoPreview, props, Nothing)
          End If
      End Function

      Private Async Function StopPreviewAsync() As Task
          _isPreviewing = False
          Await _mediaCapture.StopPreviewAsync()
          PreviewControl.Source = Nothing
      End Function

      Private Async Function ReencodeAndSavePhotoAsync(stream As IRandomAccessStream, photoOrientation As PhotoOrientation) As Task(Of String)
          Using inputStream = stream
              Dim decoder = Await BitmapDecoder.CreateAsync(inputStream)

              Dim file = Await ApplicationData.Current.LocalFolder.CreateFileAsync("photo.jpeg", CreationCollisionOption.GenerateUniqueName)

              Using outputStream = Await file.OpenAsync(FileAccessMode.ReadWrite)
                  Dim encoder = Await BitmapEncoder.CreateForTranscodingAsync(outputStream, decoder)

                  Dim properties = New BitmapPropertySet() From {
                    {"System.Photo.Orientation", New BitmapTypedValue(photoOrientation, PropertyType.UInt16)}
                  }

                  Await encoder.BitmapProperties.SetPropertiesAsync(properties)
                  Await encoder.FlushAsync()
              End Using

              Return file.Path
          End Using
      End Function

      Private Sub RegisterOrientationEventHandlers()
          If _orientationSensor IsNot Nothing Then
              AddHandler _orientationSensor.OrientationChanged, AddressOf OrientationSensor_OrientationChanged
              _deviceOrientation = _orientationSensor.GetCurrentOrientation()
          End If

          AddHandler _displayInformation.OrientationChanged, AddressOf DisplayInformation_OrientationChanged
          _displayOrientation = _displayInformation.CurrentOrientation
      End Sub

      Private Sub UnregisterOrientationEventHandlers()
          If _orientationSensor IsNot Nothing Then
              RemoveHandler _orientationSensor.OrientationChanged, AddressOf OrientationSensor_OrientationChanged
          End If

          RemoveHandler _displayInformation.OrientationChanged, AddressOf DisplayInformation_OrientationChanged
      End Sub

      Private Sub OrientationSensor_OrientationChanged(sender As SimpleOrientationSensor, args As SimpleOrientationSensorOrientationChangedEventArgs)
          If args.Orientation <> SimpleOrientation.Faceup AndAlso args.Orientation <> SimpleOrientation.Facedown Then
              _deviceOrientation = args.Orientation
          End If
      End Sub

      Private Async Sub DisplayInformation_OrientationChanged(sender As DisplayInformation, args As Object)
          _displayOrientation = sender.CurrentOrientation
          Await SetPreviewRotationAsync()
      End Sub

      Private Shared Sub OnPanelChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
          Dim ctrl = DirectCast(d, CameraControl)

          If ctrl.IsInitialized Then
              ctrl.CleanAndInitialize()
          End If
        End Sub
    End Class
End Namespace
