using ItemNamespace.uct.ItemName;
namespace ItemNamespace.Shell
{
    public class ShellViewModel : ViewModelBase
    {
        public ShellViewModel() 
        { 
            //^^
            _navigationItems.Add(new ShellNavigationItem("LOC_ANCHOR:Shell_uct.ItemName~uct.ItemName", Char.ConvertFromUtf32(0xE130), typeof(uct.ItemNameViewModel).FullName));
            SelectedItem = NavigationItems.FirstOrDefault();
        }
    }
}
