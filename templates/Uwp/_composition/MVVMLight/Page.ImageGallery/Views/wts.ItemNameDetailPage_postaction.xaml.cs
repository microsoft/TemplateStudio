//{[{
using Param_ItemNamespace.ViewModels;
//}]}
namespace Param_ItemNamespace.Views
{
    public sealed partial class wts.ItemNameDetailPage : Page
    {
        //{[{
        private wts.ItemNameDetailViewModel ViewModel
        {
            get { return ViewModelLocator.Current.wts.ItemNameDetailViewModel; }
        }

        public NavigationServiceEx NavigationService => ViewModelLocator.Current.NavigationService;        

        //}]}
    }
}
