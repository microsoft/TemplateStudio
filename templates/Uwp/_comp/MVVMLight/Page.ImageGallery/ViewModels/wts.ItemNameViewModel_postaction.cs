//{[{
using GalaSoft.MvvmLight.Command;
//}]}

namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameViewModel : ViewModelBase
    {
        //{[{
        public NavigationServiceEx NavigationService => ViewModelLocator.Current.NavigationService;
        //}]}

        private void OnItemSelected(ItemClickEventArgs args)
        {
//^^
//{[{
            NavigationService.Navigate(typeof(wts.ItemNameDetailViewModel).FullName, selected.ID);
//}]}
        }
    }
}
