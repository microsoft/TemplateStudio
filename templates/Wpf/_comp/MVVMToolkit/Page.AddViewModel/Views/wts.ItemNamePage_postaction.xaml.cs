//{[{
using Param_RootNamespace.ViewModels;
//}]}

namespace Param_RootNamespace.Views
{
    public partial class wts.ItemNamePage : Page
    {
        public wts.ItemNamePage(/*{[{*/wts.ItemNameViewModel viewModel/*}]}*/)
        {
            InitializeComponent();
//{[{
            DataContext = viewModel;
//}]}
        }
    }
}
