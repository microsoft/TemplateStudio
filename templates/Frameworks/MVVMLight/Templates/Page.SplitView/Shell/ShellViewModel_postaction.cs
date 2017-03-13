using ItemNamespace.uct.ItemName;
namespace ItemNamespace.Shell
{
    public class ShellViewModel : ViewModelBase
    {
        public ShellViewModel() 
        { 
            //^^
            _navigationItems.Add(new ShellNavigationItem("Shell_uct.ItemName".GetLocalized(), Char.ConvertFromUtf32(0xE130), typeof(uct.ItemNameViewModel).FullName));
            SelectedItem = NavigationItems.FirstOrDefault();
        }
    }
}
