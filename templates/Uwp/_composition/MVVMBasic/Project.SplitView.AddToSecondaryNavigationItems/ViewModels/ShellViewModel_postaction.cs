//{**
//This code block adds the wts.ItemNamePage to the _secondaryItems of the ShellViewModel.
//**}
using Param_ItemNamespace.Views;
namespace Param_ItemNamespace.ViewModels
{
    public class ShellViewModel : Observable
    {
        private void PopulateNavItems()
        {
            //^^
            //{[{
            _secondaryItems.Add(ShellNavigationItem.FromType<wts.ItemNamePage>("Shell_wts.ItemName".GetLocalized(), Symbol.Setting));
            //}]}
        }
    }
}
