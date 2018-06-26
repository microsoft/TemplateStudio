namespace Param_ItemNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page, Iwts.ItemNamePage
    {
        public wts.ItemNamePage()
        {
            InitializeComponent();
        }

        public GridView GetGridView() => gridView;

        //{[{
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                await ViewModel.LoadAnimationAsync();
            }
        }
        //}]}
    }
}