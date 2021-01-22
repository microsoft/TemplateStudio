using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Param_RootNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page
    {
        public wts.ItemNamePage()
        {
            InitializeComponent();
        }

        private void OnViewStateChanged(object sender, MasterDetailsViewState e)
        {
            if (e == MasterDetailsViewState.Both)
            {
                ViewModel.EnsureItemSelected();
            }
        }
    }
}
