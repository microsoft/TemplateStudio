using Microsoft.UI.Xaml.Controls;

namespace Param_RootNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page
    {
        public wts.ItemNamePage()
        {
            InitializeComponent();
            ViewModel.Initialize(MasterDetailsViewControl);
        }
    }
}
