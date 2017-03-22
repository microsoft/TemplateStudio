using ItemNamespace.Model;
using ItemNamespace.View;
namespace ItemNamespace.ViewModel
{
    public class ShellViewModel : Observable
    {
        private void PopulateNavItems()
        {
            //^^
            _navigationItems.Add(ShellNavigationItem.FromType<uct.ItemNamePage>("Shell_uct.ItemName".GetLocalized(), Symbol.Document));
        }
    }
}
