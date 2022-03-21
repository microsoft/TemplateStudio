//{[{
using Param_RootNamespace.ViewModels;
//}]}

namespace Param_RootNamespace.Views
{
    public partial class wts.ItemNameDetailPage : Page
    {
        public wts.ItemNameDetailPage(/*{[{*/wts.ItemNameDetailViewModel viewModel/*}]}*/)
        {
            InitializeComponent();
//{[{
            DataContext = viewModel;
//}]}
        }
    }
}
