//{[{
using Param_ItemNamespace.Core.Models;
using Param_ItemNamespace.ViewModels;
using Windows.UI.Xaml.Navigation;
//}]}
namespace Param_ItemNamespace.Views
{
    public sealed partial class wts.ItemNameDetailPage : Page
    {
//{[{
        public wts.ItemNameDetailViewModel ViewModel { get; } = new wts.ItemNameDetailViewModel();

//}]}
        public wts.ItemNameDetailPage()
        {            
        }

//{[{
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.Initialize(e.Parameter as SampleOrder);
        }

//}]}
    }
}
