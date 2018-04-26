using Param_ItemNamespace.ViewModels;

namespace Param_ItemNamespace.Views
{
    public sealed partial class SharedDataStorageItemsView
    {
        public SharedDataStorageItemsView()
        {
            InitializeComponent();
        }

        public SharedDataStorageItemsViewModel ViewModel => DataContext as SharedDataStorageItemsViewModel;
    }
}
