using System.Collections.ObjectModel;
using System.Windows.Input;
using uct.ItemName.Models;
using uct.ItemName.Helper;

namespace uct.ItemName.ViewModel
{
    public class PivotViewModel : Observable
    {
        private ObservableCollection<PivotTabbedItem> _items = new ObservableCollection<PivotTabbedItem>();
        public ObservableCollection<PivotTabbedItem> Items
        {
            get { return _items; }
            set { Set(ref _items, value); }
        }
        
        public PivotViewModel() 
        {
            //TODO: UWPTemplates -> Show pages in Pivot by adding a navigation item for each page with its name.
            //Edit String/en-US/Resources.resw: Add a menu item title for each page
        }
    }
}