//{[{
using Param_ItemNamespace.ViewModels;
//}]}
namespace Param_ItemNamespace.Views
{
    public sealed partial class wts.ItemNameDetailPage : Page
    {
        //{[{
        public NavigationServiceEx NavigationService
        {
            get
            {
                return CommonServiceLocator.ServiceLocator.Current.GetInstance<NavigationServiceEx>();
            }
        }

        private wts.ItemNameDetailViewModel ViewModel
        {
            get { return DataContext as wts.ItemNameDetailViewModel; }
        }

        //}]}
    }
}
