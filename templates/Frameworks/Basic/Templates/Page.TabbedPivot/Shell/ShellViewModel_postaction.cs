namespace ItemNamespace.Shell
{
    public class ShellViewModel : Observable
    {
        public ShellViewModel()
        {
            //^^
            Items.Add(new ShellTabbedItem("Shell_uct.ItemName".GetLocalized(), new uct.ItemName.uct.ItemNamePage()));
        }
    }
}