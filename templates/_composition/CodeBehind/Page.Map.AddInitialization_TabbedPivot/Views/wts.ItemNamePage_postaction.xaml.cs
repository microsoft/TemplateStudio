using Windows.UI.Xaml;
namespace ItemNamespace.Views
{
{
    public sealed partial class wts.ItemNamePage : Page, INotifyPropertyChanged
    {
        public wts.ItemNamePage()
        {
            Loaded += OnLoaded;
            Unloaded += OnUnLoaded;
            InitializeComponent();
        }        
        
        //{[{
        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await InitializeAsync();
        }

        private void OnUnLoaded(object sender, RoutedEventArgs e)
        {
            Cleanup();
        }         
        //}]}
    }
}
