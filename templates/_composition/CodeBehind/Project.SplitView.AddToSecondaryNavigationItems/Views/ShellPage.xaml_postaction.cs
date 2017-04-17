namespace ItemNamespace.Views
{
    public sealed partial class ShellPage : Page, INotifyPropertyChanged
    {
        private void PopulateNavItems()
        {
            //^^
            _secondaryItems.Add(ShellNavigationItem.FromType<wts.ItemNamePage>("Shell_wts.ItemName".GetLocalized(), Symbol.Document));
        }
    }
}
