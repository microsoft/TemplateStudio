using ItemNamespace.Helpers;
namespace ItemNamespace.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private void PopulateNavItems()
        {
            //^^
            _primaryItems.Add(new ShellNavigationItem("Shell_wts.ItemName".GetLocalized(), Symbol.Document, typeof(wts.ItemNameViewModel).FullName));
        }
    }
}