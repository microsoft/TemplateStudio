//{[{
using Windows.UI.Xaml;
//}]}
namespace Param_ItemNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page, INotifyPropertyChanged
    {
        public wts.ItemNamePage()
        {
            InitializeComponent();
            //{[{
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            //}]}
        }

        //{[{
        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await InitializeAsync();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Cleanup();
        }
        //}]}
    }
}
