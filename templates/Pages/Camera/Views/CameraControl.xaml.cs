using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Devices.Enumeration;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Param_ItemNamespace.Views
{
    public sealed partial class CameraControl
    {
        public static readonly DependencyProperty PanelProperty =
           DependencyProperty.Register("Panel", typeof(Panel), typeof(CameraControl), new PropertyMetadata(Panel.Front, OnPanelChanged));

        public static readonly DependencyProperty IsInitializedProperty =
            DependencyProperty.Register("IsInitialized", typeof(bool), typeof(CameraControl), new PropertyMetadata(false));

        // Rotation metadata to apply to the preview stream and recorded videos (MF_MT_VIDEO_ROTATION)
        // Reference: http://msdn.microsoft.com/en-us/library/windows/apps/xaml/hh868174.aspx
        private readonly Guid _rotationKey = new Guid("C380465D-2271-428C-9B83-ECEA3B4A85C1");
        private readonly DisplayInformation _displayInformation = DisplayInformation.GetForCurrentView();
        private readonly SimpleOrientationSensor _orientationSensor = SimpleOrientationSensor.GetDefault();
        private MediaCapture _mediaCapture;
        private bool _isPreviewing;
        private bool _mirroringPreview;
        private SimpleOrientation _deviceOrientation = SimpleOrientation.NotRotated;
        private DisplayOrientations _displayOrientation = DisplayOrientations.Portrait;
        private DeviceInformationCollection _cameraDevices;

        public CameraControl()
        {
            InitializeComponent();
        }

        public bool CanSwitch => _cameraDevices?.Count > 1;

        public Panel Panel
        {
            get { return (Panel)GetValue(PanelProperty); }
            set { SetValue(PanelProperty, value); }
        }

        public bool IsInitialized
        {
            get { return (bool)GetValue(IsInitializedProperty); }
            private set { SetValue(IsInitializedProperty, value); }
        }

        public async Task InitializeAsync()
        {
            await InitializeCameraAsync();
        }

        /// <summary>
        /// Takes a photo to a StorageFile and adds rotation metadata to it
        /// </summary>
        /// <returns>Photo path</returns>
        public async Task<string> TakePhotoAsync()
        {
            using (var stream = new InMemoryRandomAccessStream())
            {
                await _mediaCapture.CapturePhotoToStreamAsync(ImageEncodingProperties.CreateJpeg(), stream);

                var photoOrientation = ConvertOrientationToPhotoOrientation(GetCameraOrientation(_displayInformation, _deviceOrientation));

                return await ReencodeAndSavePhotoAsync(stream, photoOrientation);
            }
        }

        public void Cleanup()
        {
            Task.Run(async () => await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                await CleanupCameraAsync();
            }));
        }

        public void CleanAndInitialize()
        {
            Task.Run(async () => await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                await CleanupCameraAsync();
                await InitializeAsync();
            }));
        }

        private static void OnPanelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CameraControl)d).CleanAndInitialize();
        }

        /// <summary>
        /// Initializes the MediaCapture, registers events, gets camera device information for mirroring and rotating, starts preview and unlocks the UI
        /// </summary>
        /// <returns>Task</returns>
        private async Task InitializeCameraAsync()
        {
            if (_mediaCapture == null)
            {
                _mediaCapture = new MediaCapture();
                _mediaCapture.Failed += MediaCapture_Failed;

                _cameraDevices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
                if (_cameraDevices == null)
                {
                    throw new NotSupportedException();
                }

                try
                {
                    var device = _cameraDevices.FirstOrDefault(camera => camera.EnclosureLocation?.Panel == Panel);

                    var cameraId = device != null ? device.Id : _cameraDevices.First().Id;

                    await _mediaCapture.InitializeAsync(new MediaCaptureInitializationSettings { VideoDeviceId = cameraId });

                    if (Panel == Panel.Back)
                    {
                        _mediaCapture.SetRecordRotation(VideoRotation.Clockwise90Degrees);
                        _mediaCapture.SetPreviewRotation(VideoRotation.Clockwise90Degrees);
                    }

                    IsInitialized = true;
                }
                catch (UnauthorizedAccessException ex)
                {
                    throw ex;
                }

                if (IsInitialized)
                {
                    _mirroringPreview = true;
                    PreviewControl.Source = _mediaCapture;
                    RegisterOrientationEventHandlers();
                    await StartPreviewAsync();
                }
            }
        }

        private void MediaCapture_Failed(MediaCapture sender, MediaCaptureFailedEventArgs errorEventArgs)
        {
            Cleanup();
        }

        /// <summary>
        /// Cleans up the camera resources (after stopping any video recording and/or preview if necessary) and unregisters from MediaCapture events
        /// </summary>
        /// <returns>Task</returns>
        private async Task CleanupCameraAsync()
        {
            if (IsInitialized)
            {
                if (_isPreviewing)
                {
                    await StopPreviewAsync();
                    UnregisterOrientationEventHandlers();
                }

                IsInitialized = false;
            }

            if (_mediaCapture != null)
            {
                _mediaCapture.Failed -= MediaCapture_Failed;
                _mediaCapture.Dispose();
                _mediaCapture = null;
            }
        }

        /// <summary>
        /// Starts the preview and adjusts it for for rotation and mirroring after making a request to keep the screen on
        /// </summary>
        /// <returns>Task</returns>
        private async Task StartPreviewAsync()
        {
            PreviewControl.Source = _mediaCapture;
            PreviewControl.FlowDirection = _mirroringPreview ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

            await _mediaCapture.StartPreviewAsync();
            _isPreviewing = true;

            if (_isPreviewing)
            {
                await SetPreviewRotationAsync();
            }
        }

        private async Task SetPreviewRotationAsync()
        {
            _displayOrientation = _displayInformation.CurrentOrientation;
            int rotationDegrees = ConvertDisplayOrientationToDegrees(_displayOrientation);

            if (_mirroringPreview)
            {
                rotationDegrees = (360 - rotationDegrees) % 360;
            }

            var props = _mediaCapture.VideoDeviceController.GetMediaStreamProperties(MediaStreamType.VideoPreview);
            props.Properties.Add(_rotationKey, rotationDegrees);
            await _mediaCapture.SetEncodingPropertiesAsync(MediaStreamType.VideoPreview, props, null);
        }

        /// <summary>
        /// Stops the preview and deactivates a display request, to allow the screen to go into power saving modes
        /// </summary>
        /// <returns>Task</returns>
        private async Task StopPreviewAsync()
        {
            _isPreviewing = false;
            await _mediaCapture.StopPreviewAsync();
            PreviewControl.Source = null;
        }

        /// <summary>
        /// Applies the given orientation to a photo stream and saves it as a StorageFile
        /// </summary>
        /// <param name="stream">The photo stream</param>
        /// <param name="photoOrientation">The orientation metadata to apply to the photo</param>
        /// <returns>File path</returns>
        private async Task<string> ReencodeAndSavePhotoAsync(IRandomAccessStream stream, PhotoOrientation photoOrientation)
        {
            using (var inputStream = stream)
            {
                var decoder = await BitmapDecoder.CreateAsync(inputStream);
                
                // TODO WTS: Set the file path and the name of the photo
                var file = await Package.Current.InstalledLocation.CreateFileAsync("photo.jpeg", CreationCollisionOption.GenerateUniqueName);

                using (var outputStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var encoder = await BitmapEncoder.CreateForTranscodingAsync(outputStream, decoder);

                    var properties = new BitmapPropertySet { { "System.Photo.Orientation", new BitmapTypedValue(photoOrientation, PropertyType.UInt16) } };

                    await encoder.BitmapProperties.SetPropertiesAsync(properties);
                    await encoder.FlushAsync();
                }

                return file.Path;
            }
        }

        private void RegisterOrientationEventHandlers()
        {
            if (_orientationSensor != null)
            {
                _orientationSensor.OrientationChanged += OrientationSensor_OrientationChanged;
                _deviceOrientation = _orientationSensor.GetCurrentOrientation();
            }

            _displayInformation.OrientationChanged += DisplayInformation_OrientationChanged;
            _displayOrientation = _displayInformation.CurrentOrientation;
        }

        private void UnregisterOrientationEventHandlers()
        {
            if (_orientationSensor != null)
            {
                _orientationSensor.OrientationChanged -= OrientationSensor_OrientationChanged;
            }

            _displayInformation.OrientationChanged -= DisplayInformation_OrientationChanged;
        }

        private void OrientationSensor_OrientationChanged(SimpleOrientationSensor sender, SimpleOrientationSensorOrientationChangedEventArgs args)
        {
            if (args.Orientation != SimpleOrientation.Faceup && args.Orientation != SimpleOrientation.Facedown)
            {
                _deviceOrientation = args.Orientation;
            }
        }

        private async void DisplayInformation_OrientationChanged(DisplayInformation sender, object args)
        {
            _displayOrientation = sender.CurrentOrientation;

            if (_isPreviewing)
            {
                await SetPreviewRotationAsync();
            }
        }

        private SimpleOrientation GetCameraOrientation(DisplayInformation displayInformation, SimpleOrientation deviceOrientation)
        {
            var result = deviceOrientation;

            // Account for the fact that, on portrait-first devices, the camera sensor is mounted at a 90 degree offset to the native orientation
            if (displayInformation.NativeOrientation == DisplayOrientations.Portrait)
            {
                switch (result)
                {
                    case SimpleOrientation.Rotated90DegreesCounterclockwise:
                        result = SimpleOrientation.NotRotated;
                        break;
                    case SimpleOrientation.Rotated180DegreesCounterclockwise:
                        result = SimpleOrientation.Rotated90DegreesCounterclockwise;
                        break;
                    case SimpleOrientation.Rotated270DegreesCounterclockwise:
                        result = SimpleOrientation.Rotated180DegreesCounterclockwise;
                        break;
                    case SimpleOrientation.NotRotated:
                        result = SimpleOrientation.Rotated270DegreesCounterclockwise;
                        break;
                }
            }

            // If the preview is being mirrored for a front-facing camera, then the rotation should be inverted
            if (_mirroringPreview)
            {
                // This only affects the 90 and 270 degree cases, because rotating 0 and 180 degrees is the same clockwise and counter-clockwise
                switch (result)
                {
                    case SimpleOrientation.Rotated90DegreesCounterclockwise:
                        return SimpleOrientation.Rotated270DegreesCounterclockwise;
                    case SimpleOrientation.Rotated270DegreesCounterclockwise:
                        return SimpleOrientation.Rotated90DegreesCounterclockwise;
                }
            }

            return result;
        }

        private int ConvertDisplayOrientationToDegrees(DisplayOrientations orientation)
        {
            switch (orientation)
            {
                case DisplayOrientations.Portrait:
                    return 90;
                case DisplayOrientations.LandscapeFlipped:
                    return 180;
                case DisplayOrientations.PortraitFlipped:
                    return 270;
                case DisplayOrientations.Landscape:
                default:
                    return 0;
            }
        }

        private PhotoOrientation ConvertOrientationToPhotoOrientation(SimpleOrientation orientation)
        {
            switch (orientation)
            {
                case SimpleOrientation.Rotated90DegreesCounterclockwise:
                    return PhotoOrientation.Rotate90;
                case SimpleOrientation.Rotated180DegreesCounterclockwise:
                    return PhotoOrientation.Rotate180;
                case SimpleOrientation.Rotated270DegreesCounterclockwise:
                    return PhotoOrientation.Rotate270;
                case SimpleOrientation.NotRotated:
                default:
                    return PhotoOrientation.Normal;
            }
        }
    }
}