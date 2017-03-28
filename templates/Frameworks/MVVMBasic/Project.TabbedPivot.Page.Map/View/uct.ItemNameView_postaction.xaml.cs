namespace ItemNamespace.View
{
    public sealed partial class uct.ItemNameView : Page
    {
        public uct.ItemNameView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }        
        
        //{[{
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null)
            {
                throw new ArgumentNullException("ViewModel");
            }
            
            ViewModel.Initialize(mapControl);
        }        
        //}]}
    }
}
