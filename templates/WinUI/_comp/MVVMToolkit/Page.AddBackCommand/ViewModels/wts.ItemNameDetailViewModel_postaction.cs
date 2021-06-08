//{[{
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Param_RootNamespace.Contracts.Services;
//}]}

namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameDetailViewModel : ObservableRecipient, INavigationAware
    {
//{[{
        private readonly INavigationService _navigationService;
//}]}
//^^
//{[{
        private ICommand _goBackCommand;

        public ICommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand(OnGoBack));
//}]}

        public wts.ItemNameDetailViewModel(/*{[{*/INavigationService navigationService/*}]}*/)
        {
//{[{
            _navigationService = navigationService;
//}]}
        }

//^^
//{[{
        private void OnGoBack()
        {
            if (_navigationService.CanGoBack)
            {
                _navigationService.GoBack();
            }
        }
//}]}
    }
}