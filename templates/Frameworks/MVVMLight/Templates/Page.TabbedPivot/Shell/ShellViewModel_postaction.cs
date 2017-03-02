using ItemNamespace.ItemName;
namespace ItemNamespace.Shell
{
    public class ShellViewModel : ViewModelBase
    {
        public IEnumerable<ShellTabbedItem> Items
        {
            get
            {
                //^^
                yield return new ShellTabbedItem("LOC_ANCHOR:Shell_ItemName~ItemName", new ItemName.ItemNamePage());
            }
        }
    }
}
