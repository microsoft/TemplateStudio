using Windows.UI.Xaml;
namespace ItemNamespace.Views
{
{
    public sealed partial class uct.ItemNamePage : Page, INotifyPropertyChanged
    {
        public uct.ItemNamePage()
        {
            Loaded += OnLoaded;
            InitializeComponent();
        }        
        
        //{[{
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //TODO UWPTemplates: Set your map service token. If you don't have it, request at https://www.bingmapsportal.com/            
            mapControl.MapServiceToken = "";
            
            AddMapIcon(Center, "Microsoft Corporation");
        }        
        //}]}
    }
}
