using System;
using System.Windows.Input;

using Prism.Commands;
using Prism.Windows.Mvvm;

using Windows.UI.Xaml.Media.Imaging;

using WTSGeneratedNavigation.EventHandlers;
using WTSGeneratedNavigation.Helpers;

namespace WTSGeneratedNavigation.ViewModels
{
    public class CameraViewModel : ViewModelBase
    {
        private ICommand _photoTakenCommand;
        private BitmapImage _photo;

        public BitmapImage Photo
        {
            get { return _photo; }
            set { SetProperty(ref _photo, value); }
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
