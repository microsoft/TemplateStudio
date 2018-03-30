//{[{
using Windows.UI.Xaml.Navigation;
//}]}
namespace Param_ItemNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page, INotifyPropertyChanged
    {
        public wts.ItemNamePage()
        {
        }

        //{[{
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Initialize();
        }
        //}]}
    }
}
