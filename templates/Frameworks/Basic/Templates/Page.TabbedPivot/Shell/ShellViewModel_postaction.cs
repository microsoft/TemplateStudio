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
                yield return new ShellTabbedItem("LOC_ANCHOR:Shell_uct.ItemName~uct.ItemName", new uct.ItemName.uct.ItemNamePage());
            }
        }
    }
}
