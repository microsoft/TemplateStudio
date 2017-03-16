namespace ItemNamespace.Shell
{
    public class ShellViewModel : ViewModelBase
    {
        public ShellViewModel()
        {
            //^^
            Items.Add(new ShellTabbedItem("Shell_uct.ItemName".GetLocalized(), new uct.ItemName.uct.ItemNamePage()));
        }
    }
}