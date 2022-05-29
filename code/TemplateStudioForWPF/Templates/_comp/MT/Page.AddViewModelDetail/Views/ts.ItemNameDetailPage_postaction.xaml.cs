//{[{
using Param_RootNamespace.ViewModels;
//}]}

namespace Param_RootNamespace.Views
{
    public partial class ts.ItemNameDetailPage : Page
    {
        public ts.ItemNameDetailPage(/*{[{*/ts.ItemNameDetailViewModel viewModel/*}]}*/)
        {
            InitializeComponent();
//{[{
            DataContext = viewModel;
//}]}
        }
    }
}
