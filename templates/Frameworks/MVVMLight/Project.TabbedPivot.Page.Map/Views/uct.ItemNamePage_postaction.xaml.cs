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
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.InitializeAsync(mapControl);
        }
        //}]}
    }
}
