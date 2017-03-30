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
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Initialize();
        }        
        //}]}
    }
}
