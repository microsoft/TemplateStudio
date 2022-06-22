//{[{
using Param_RootNamespace.ViewModels;
//}]}

namespace Param_RootNamespace.Views;

public partial class ts.ItemNamePage : Page
{
    public ts.ItemNamePage(/*{[{*/ts.ItemNameViewModel viewModel/*}]}*/)
    {
        InitializeComponent();
//{[{
        DataContext = viewModel;
//}]}
    }
}
