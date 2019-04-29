using System;
using Prism.Windows.Mvvm;
using Windows.UI.Xaml.Media.Imaging;

namespace Param_RootNamespace.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private string _name;
        private string _userPrincipalName;
        private BitmapImage _photo;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string UserPrincipalName
        {
            get => _userPrincipalName;
            set => SetProperty(ref _userPrincipalName, value);
        }

        public BitmapImage Photo
        {
            get => _photo;
            set => SetProperty(ref _photo, value);
        }

        public UserViewModel()
        {
        }
    }
}
