using Windows.UI.Xaml.Controls;

namespace uct.ItemName.Shell
{
    public class ShellTabbedItem
    {
        public string Title { get; set; }
        public Page Page { get; set; }

        public ShellTabbedItem(string title, Page page)
        {
            this.Title = title;
            this.Page = page;
        }
    }
}