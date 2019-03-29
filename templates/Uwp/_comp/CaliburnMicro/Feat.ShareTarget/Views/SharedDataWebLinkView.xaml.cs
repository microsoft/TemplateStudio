using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace.Views
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
