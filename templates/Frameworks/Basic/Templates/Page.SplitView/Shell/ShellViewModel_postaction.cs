using ItemNamespace.uct.ItemName;
namespace ItemNamespace.Shell
{
    public class ShellViewModel : Observable
    {
        public ShellViewModel() 
        {
            //^^
            _navigationItems.Add(ShellNavigationItem.FromType<uct.ItemNamePage>("Shell_uct.ItemName".GetLocalized(), Symbol.Document));
            SelectedItem = NavigationItems.FirstOrDefault();
        }
    }
}
