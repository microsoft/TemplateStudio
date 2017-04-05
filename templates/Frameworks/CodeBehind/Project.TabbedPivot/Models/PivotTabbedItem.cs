using Windows.UI.Xaml.Controls;

namespace uct.TabbedPivotProject.Models
{
    public class PivotTabbedItem
    {
        public string Title { get; set; }
        public Page Page { get; set; }

        public PivotTabbedItem(string title, Page page)
        {
            this.Title = title;
            this.Page = page;
        }
    }
}