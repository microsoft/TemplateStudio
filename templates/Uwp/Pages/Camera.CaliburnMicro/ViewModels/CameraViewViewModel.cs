using System;

using Param_ItemNamespace.EventHandlers;
using Caliburn.Micro;
using Windows.UI.Xaml.Media.Imaging;

namespace Param_ItemNamespace.ViewModels
{
    public class CameraViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private BitmapImage _photo;

        public BitmapImage Photo
        {
            get { return _photo; }
            set { Set(ref _photo, value); }
        }

        public void OnPhotoTaken(CameraControlEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.Photo))
            {
                Photo = new BitmapImage(new Uri(args.Photo));
            }
        }
    }
}