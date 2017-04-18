using ItemNamespace.ViewModels;
namespace ItemNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page
    {
        //{[{
        private wts.ItemNameViewModel ViewModel
        {
            get { return DataContext as wts.ItemNameViewModel; }
        }

        //}]}
    }
}
