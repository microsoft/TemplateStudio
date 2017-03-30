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
        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync(AdaptiveStates.CurrentState);
        }        
        //}]}
    }
}
