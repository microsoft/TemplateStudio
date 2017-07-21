//{**
//This code block adds the wts.ItemNameViewModel to the _primaryItems of the ShellViewModel.
//**}
//{[{
using Param_ItemNamespace.Helpers;
//}]}
namespace Param_ItemNamespace.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private void PopulateNavItems()
        {
            //^^
            //{[{
            _primaryItems.Add(new ShellNavigationItem("Shell_wts.ItemName".GetLocalized(), Symbol.Document, typeof(wts.ItemNameViewModel).FullName));
            //}]}
        }
    }
}