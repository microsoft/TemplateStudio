using Param_RootNamespace.EventHandlers;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Param_RootNamespace.Views
{
    public sealed partial class CameraViewPage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        public CameraViewPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await cameraControl.InitializeCameraAsync();
        }

        protected override async void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            await cameraControl.CleanupCameraAsync();
        }

        private void CameraControl_PhotoTaken(object sender, CameraControlEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Photo))
            {
                Photo.Source = new BitmapImage(new Uri(e.Photo));
            }
        }
    }
}
