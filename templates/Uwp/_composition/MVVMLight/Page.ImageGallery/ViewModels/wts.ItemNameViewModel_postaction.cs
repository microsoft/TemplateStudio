//{[{
using GalaSoft.MvvmLight.Command;
//}]}

namespace Param_ItemNamespace.ViewModels
{
    public class wts.ItemNameViewModel : ViewModelBase
    {
        //{[{
        public NavigationServiceEx NavigationService => ViewModelLocator.Current.NavigationService;
        //}]}

        private void OnsItemSelected(ItemClickEventArgs args)
        {
//^^
//{[{
            NavigationService.Navigate(typeof(wts.ItemNameDetailViewModel).FullName, selected);
//}]}
        }
    }
}
