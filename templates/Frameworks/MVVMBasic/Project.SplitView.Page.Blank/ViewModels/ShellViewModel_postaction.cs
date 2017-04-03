using ItemNamespace.Models;
using ItemNamespace.Views;
using ItemNamespace.Helper;
namespace ItemNamespace.ViewModels
{
    public class ShellViewModel : Observable
    {
        private void PopulateNavItems()
        {
            //^^
            _primaryItems.Add(ShellNavigationItem.FromType<uct.ItemNamePage>("Shell_uct.ItemName".GetLocalized(), Symbol.Document));
        }
    }
}
