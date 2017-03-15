using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace uct.ItemName.Shell
{
    public class ShellViewModel : ViewModelBase
    {
        private ObservableCollection<ShellTabbedItem> _items = new ObservableCollection<ShellTabbedItem>();
        public ObservableCollection<ShellTabbedItem> Items
        {
            get { return _items; }
            set { Set(ref _items, value); }
        }

        public ShellViewModel()
        {
            //TODO: UWPTemplates -> Show pages in Pivot by adding a navigation item for each page with its name.
            //Edit String/en-US/Resources.resw: Add a menu item title for each page
        }
    }
}
