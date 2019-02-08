//{[{
using Param_RootNamespace.ViewModels;
//}]}
namespace Param_RootNamespace.Views
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
