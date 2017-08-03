using System;
using System.Windows.Input;
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

        public ICommand CapturedCommand { get; private set; }
        
        public CameraViewViewModel()
        {
            CapturedCommand = new RelayCommand<string>(OnCaptured);
        }

        private void OnCaptured(string path)
        {
            Photo = new BitmapImage(new Uri(path));
        }
    }
}
