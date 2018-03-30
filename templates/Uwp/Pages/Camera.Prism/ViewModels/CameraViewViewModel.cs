using System;
using System.Windows.Input;
using Prism.Commands;
using Param_ItemNamespace.Helpers;
using Windows.UI.Xaml.Media.Imaging;
using Param_ItemNamespace.EventHandlers;

namespace Param_ItemNamespace.ViewModels
{
    public class CameraViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private ICommand _photoTakenCommand;
        private BitmapImage _photo;

        public BitmapImage Photo
        {
            get { return _photo; }
            set { Param_Setter(ref _photo, value); }
        }

        public ICommand PhotoTakenCommand => _photoTakenCommand ?? (_photoTakenCommand = new DelegateCommand<CameraControlEventArgs>(OnPhotoTaken));

        private void OnPhotoTaken(CameraControlEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.Photo))
            {
                Photo = new BitmapImage(new Uri(args.Photo));
            }
        }
    }
}
