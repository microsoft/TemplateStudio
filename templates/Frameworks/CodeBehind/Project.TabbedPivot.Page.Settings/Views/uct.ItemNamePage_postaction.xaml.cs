namespace ItemNamespace.Views
{
    public sealed partial class uct.ItemNamePage : Page, INotifyPropertyChanged
    {
        public uct.ItemNamePage()
        {
            Initialize();
            Loaded += OnLoaded;
            InitializeComponent();
        }
        
        //{[{
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AppDescription = GetAppDescription();
        }        
        //}]}
    }
}
