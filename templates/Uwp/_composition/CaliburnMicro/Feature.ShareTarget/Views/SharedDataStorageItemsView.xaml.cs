using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace.Views
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
