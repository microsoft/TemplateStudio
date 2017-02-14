using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;

namespace ItemName.Shell
{
    public class ShellTabbedItem
    {
        public string Title { get; set; }
        public Page Page { get; set; }

        public ShellTabbedItem(string resource, Page page)
        {
            ResourceLoader resourceLoader = new ResourceLoader();
            this.Title = resourceLoader.GetString(resource);
            this.Page = page;
        }
    }
}