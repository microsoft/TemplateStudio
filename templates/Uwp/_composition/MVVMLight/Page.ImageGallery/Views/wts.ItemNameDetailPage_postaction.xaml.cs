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
                return ViewModelLocator.Current.NavigationService;
            }
        }

        private wts.ItemNameDetailViewModel ViewModel
        {
            get { return ViewModelLocator.Current.wts.ItemNameDetailViewModel; }
        }

        //}]}
    }
}
