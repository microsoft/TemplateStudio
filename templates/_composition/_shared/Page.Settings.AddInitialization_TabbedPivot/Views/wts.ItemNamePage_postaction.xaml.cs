using Windows.UI.Xaml;
namespace ItemNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page
    {
        public wts.ItemNamePage()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }
        
        //{[{
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Initialize();
        }        
        //}]}
    }
}
