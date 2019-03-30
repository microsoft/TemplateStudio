//{[{
using System.Windows.Input;
using Param_RootNamespace.Helpers;
using Param_RootNamespace.Services;
using GalaSoft.MvvmLight.Command;
//}]}

namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameDetailViewModel : ViewModelBase
    {
//^^
//{[{
        public static NavigationServiceEx NavigationService => ViewModelLocator.Current.NavigationService;

        private ICommand _goBackCommand;

        public ICommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand(OnGoBack));
//}]}

        public wts.ItemNameDetailViewModel()
        {
        }

//^^
//{[{
        private void OnGoBack()
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
//}]}
    }
}