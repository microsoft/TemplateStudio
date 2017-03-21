using Windows.UI.Xaml.Controls;

namespace uct.TabbedPivotProject.PivotPage
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