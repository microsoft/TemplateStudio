using ItemNamespace.uct.ItemName;
namespace ItemNamespace.Shell
{
    public class ShellViewModel : ViewModelBase
    {
        public ShellViewModel() 
        { 
            //^^
            _navigationItems.Add(new ShellNavigationItem("Shell_uct.ItemName".GetLocalized(), Symbol.Document, typeof(uct.ItemNameViewModel).FullName));
            SelectedItem = NavigationItems.FirstOrDefault();
        }
    }
}
