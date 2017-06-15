using GalaSoft.MvvmLight.Command;
using Param_ItemNamespace.Services;
namespace Param_ItemNamespace.ViewModels
{
    public class wts.ItemNameViewModel : ViewModelBase
    {
        //{[{
        public NavigationServiceEx NavigationService
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<NavigationServiceEx>();
            }
        }
        //}]}

        private void OnItemClick(ItemClickEventArgs args)
        {
            Order item = args?.ClickedItem as Order;
            if (item != null)
            {
                if (_currentState.Name == NarrowStateName)
                {
                    NavigationService.Navigate(typeof(wts.ItemNameDetailViewModel).FullName, item);
                }
                else
                {
                    Selected = item;
                }
            }
        }
    }
}