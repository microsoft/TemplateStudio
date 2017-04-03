using ItemNamespace.Helper;
namespace ItemNamespace.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private void PopulateNavItems()
        {
            //^^
            _secondaryItems.Add(new ShellNavigationItem("Shell_uct.ItemName".GetLocalized(), Symbol.Document, typeof(uct.ItemNameViewModel).FullName));
        }
    }
}
