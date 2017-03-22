namespace ItemNamespace.ViewModel
{
    public class ShellViewModel : ViewModelBase
    {
        private void PopulateNavItems()
        {
            //^^
            _navigationItems.Add(ShellNavigationItem.FromType<uct.ItemNamePage>("Shell_uct.ItemName".GetLocalized(), Symbol.Document));
        }
    }
}
