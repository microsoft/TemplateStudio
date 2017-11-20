using Param_ItemNamespace.EventHandlers;
using System;
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

        private void CameraControl_PhotoTaken(object sender, CameraControlEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Photo))
            {
                Photo.Source = new BitmapImage(new Uri(e.Photo));
            }
        }
    }
}
