//{**
//This code block adds the wts.ItemNamePage to the _primaryItems of the ShellViewModel.
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
            _primaryItems.Add(ShellNavigationItem.FromType<wts.ItemNamePage>("Shell_wts.ItemName".GetLocalized(), Symbol.Document));
            //}]}
        }
    }
}
