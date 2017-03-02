using ItemNamespace.ItemName;
namespace ItemNamespace.Shell
{
    public class ShellViewModel : ViewModelBase
    {
        public ShellViewModel() 
        { 
            //^^
            _navigationItems.Add(new ShellNavigationItem("LOC_ANCHOR:Shell_ItemName~ItemName", Char.ConvertFromUtf32(0xE130), typeof(ItemNameViewModel).FullName));
            SelectedItem = NavigationItems.FirstOrDefault();
        }
    }
}
