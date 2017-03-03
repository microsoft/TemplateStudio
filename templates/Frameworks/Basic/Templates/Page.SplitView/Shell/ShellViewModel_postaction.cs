using ItemNamespace.ItemName;
namespace ItemNamespace.Shell
{
    public class ShellViewModel : Observable
    {
        public ShellViewModel() 
        {
            //^^
            _navigationItems.Add(ShellNavigationItem.FromType<ItemNamePage>("LOC_ANCHOR:Shell_ItemName~ItemName", Char.ConvertFromUtf32(0xE130)));
            SelectedItem = NavigationItems.FirstOrDefault();
        }
    }
}
