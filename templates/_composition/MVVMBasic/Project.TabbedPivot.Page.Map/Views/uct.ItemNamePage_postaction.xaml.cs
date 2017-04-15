using Windows.UI.Xaml;
namespace ItemNamespace.Views
{
    public sealed partial class uct.ItemNamePage : Page
    {
        public uct.ItemNamePage()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        //{[{
        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitializeAsync(mapControl);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Cleanup();
        }
        //}]}
    }
}
