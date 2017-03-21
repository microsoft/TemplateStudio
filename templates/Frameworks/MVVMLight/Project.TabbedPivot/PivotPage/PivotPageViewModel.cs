using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace uct.ItemName.PivotPage
{
    public class PivotPageViewModel : ViewModelBase
    {
        private ObservableCollection<PivotTabbedItem> _items = new ObservableCollection<PivotTabbedItem>();
        public ObservableCollection<PivotTabbedItem> Items
        {
            get { return _items; }
            set { Set(ref _items, value); }
        }

        public PivotPageViewModel()
        {
            //TODO: UWPTemplates -> Show pages in Pivot by adding a navigation item for each page with its name.
            //Edit String/en-US/Resources.resw: Add a menu item title for each page
        }
    }
}
