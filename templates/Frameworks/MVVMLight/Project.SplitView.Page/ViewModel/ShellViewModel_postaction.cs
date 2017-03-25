using ItemNamespace.Extensions;

namespace ItemNamespace.ViewModel
{
    public class ShellViewModel : ViewModelBase
    {
        private void PopulateNavItems()
        {
            //^^
            _navigationItems.Add(new ShellNavigationItem("Shell_uct.ItemName".GetLocalized(), Symbol.Document, typeof(uct.ItemNameViewModel).FullName));
        }
    }
}
