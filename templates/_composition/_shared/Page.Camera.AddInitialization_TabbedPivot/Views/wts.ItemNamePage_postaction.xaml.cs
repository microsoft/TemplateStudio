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
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            //}]}
        }

        //{[{
        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitializeAsync(Camera);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Cleanup();
        }
        //}]}
    }
}
