//{[{
using Windows.UI.Xaml;
//}]}
namespace Param_ItemNamespace.Views
{
{
    public sealed partial class wts.ItemNamePage : Page, INotifyPropertyChanged
    {
        public wts.ItemNamePage()
        {
            //{[{
            Loaded += wts.ItemNamePage_Loaded;
            Unloaded += wts.ItemNamePage_Unloaded;
            //}]}
            InitializeComponent();
        }

        //{[{
        private async void wts.ItemNamePage_Loaded(object sender, RoutedEventArgs e)
        {
            await InitializeAsync();
        }

        private void wts.ItemNamePage_Unloaded(object sender, RoutedEventArgs e)
        {
            Cleanup();
        }
        //}]}
    }
}
