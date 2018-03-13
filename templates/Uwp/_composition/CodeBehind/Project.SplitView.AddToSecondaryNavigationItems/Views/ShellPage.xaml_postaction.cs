//{**
//This code block adds the wts.ItemNamePage to the _secondaryItems of the ShellPage.
//**}

namespace Param_ItemNamespace.Views
{
    public sealed partial class ShellPage : Page, INotifyPropertyChanged
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
