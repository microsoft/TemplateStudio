//{[{
using Param_RootNamespace.ViewModels;
//}]}
namespace Param_RootNamespace.Views
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
