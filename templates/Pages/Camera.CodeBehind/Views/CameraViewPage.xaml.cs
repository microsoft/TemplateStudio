using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Param_ItemNamespace.Views
{
    public sealed partial class CameraViewPage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        public CameraViewPage()
        {
            InitializeComponent();
        }

        private async void Photo_Click(object sender, RoutedEventArgs e)
        {
            Photo.Source = new BitmapImage(new Uri(await Camera.TakePhotoAsync()));
        }
    }
}