using Param_ItemNamespace.ViewModels;

namespace Param_ItemNamespace.Views
{
    public sealed partial class SharedDataWebLinkView
    {
        public SharedDataWebLinkView()
        {
            InitializeComponent();
        }

        public SharedDataWebLinkViewModel ViewModel => DataContext as SharedDataWebLinkViewModel;
    }
}
