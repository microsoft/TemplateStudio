//{[{
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Param_RootNamespace.Helpers;
using Param_RootNamespace.Services;
//}]}

namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameDetailViewModel : ObservableObject
    {
//^^
//{[{
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