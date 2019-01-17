//{[{
using System.Windows.Input;
using Param_ItemNamespace.Helpers;
using Param_ItemNamespace.Services;
//}]}

namespace Param_ItemNamespace.ViewModels
{
    public class wts.ItemNameDetailViewModel : Observable
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