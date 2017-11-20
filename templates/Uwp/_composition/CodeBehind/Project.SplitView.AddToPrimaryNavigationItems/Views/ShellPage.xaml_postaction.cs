//{**
//This code block adds the wts.ItemNamePage to the _primaryItems of the ShellPage.
//**}

namespace Param_ItemNamespace.Views
{
    public sealed partial class ShellPage : Page, INotifyPropertyChanged
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
