namespace Param_ItemNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page
    {
        public wts.ItemNamePage()
        {
        }

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