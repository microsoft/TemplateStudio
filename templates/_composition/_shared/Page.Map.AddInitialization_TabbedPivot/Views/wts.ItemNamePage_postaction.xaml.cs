//{[{
using Windows.UI.Xaml;
//}]}
namespace Param_ItemNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page
    {
        public wts.ItemNamePage()
        {
            InitializeComponent();
            //{[{
            Loaded += wts.ItemNamePage_Loaded;
            Unloaded += wts.ItemNamePage_Unloaded;
            //}]}
        }

        //{[{
        private async void wts.ItemNamePage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitializeAsync(mapControl);
        }

        private void wts.ItemNamePage_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Cleanup();
        }
        //}]}
    }
}
