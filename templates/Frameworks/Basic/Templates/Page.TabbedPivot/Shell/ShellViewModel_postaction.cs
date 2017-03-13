using ItemNamespace.uct.ItemName;
namespace ItemNamespace.Shell
{
    public class ShellViewModel : Observable
    {
        public IEnumerable<ShellTabbedItem> Items
        {
            get
            {
                //^^
                yield return new ShellTabbedItem("Shell_uct.ItemName".GetLocalized(), new uct.ItemName.uct.ItemNamePage());
            }
        }
    }
}
