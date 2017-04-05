using Windows.UI.Xaml;
namespace ItemNamespace.Views
{
    public sealed partial class uct.ItemNamePage : Page
    {
        public uct.ItemNamePage()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }
        
        //{[{
        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync(WindowStates.CurrentState);
        }        
        //}]}
    }
}
