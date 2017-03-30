using ItemNamespace.Model;
using ItemNamespace.View;
using ItemNamespace.Helper;
namespace ItemNamespace.ViewModel
{
    public class ShellViewModel : Observable
    {
        private void PopulateNavItems()
        {
            //^^
            _navigationItems.Add(ShellNavigationItem.FromType<uct.ItemNameView>("Shell_uct.ItemName".GetLocalized(), Symbol.Document));
        }
    }
}
