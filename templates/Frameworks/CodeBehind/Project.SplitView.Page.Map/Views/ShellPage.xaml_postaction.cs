namespace ItemNamespace.Views
{
    public sealed partial class ShellPage : Page, INotifyPropertyChanged
    {
        private void PopulateNavItems()
        {
            //^^
            _primaryItems.Add(ShellNavigationItem.FromType<uct.ItemNamePage>("Shell_uct.ItemName".GetLocalized(), Symbol.Document));
        }
    }
}
