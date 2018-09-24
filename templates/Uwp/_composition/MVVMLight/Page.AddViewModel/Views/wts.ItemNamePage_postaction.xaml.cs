//{[{
using Param_ItemNamespace.ViewModels;
//}]}
namespace Param_ItemNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page
    {
        //{[{
        private wts.ItemNameViewModel ViewModel
        {
            get { return ViewModelLocator.Current.wts.ItemNameViewModel; }
        }

        //}]}
    }
}
