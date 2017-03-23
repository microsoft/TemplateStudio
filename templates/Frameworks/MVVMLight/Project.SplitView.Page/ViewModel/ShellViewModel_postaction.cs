using ItemNamespace.Extensions;
namespace ItemNamespace.ViewModel
{
    public class ShellViewModel : ViewModelBase
    {
        private void PopulateNavItems()
        {
            //^^
            _navigationItems.Add(ShellNavigationItem.FromType<uct.ItemNameView>("Shell_uct.ItemName".GetLocalized(), Symbol.Document));
        }
    }
}
