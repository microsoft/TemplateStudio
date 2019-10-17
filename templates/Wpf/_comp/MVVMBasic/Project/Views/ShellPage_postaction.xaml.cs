//{[{
using Param_RootNamespace.ViewModels;
//}]}

namespace Param_RootNamespace.Views
{
    public partial class ShellPage : MetroWindow, IShellPage
    {
        public ShellPage(/*{[{*/ShellViewModel viewModel/*}]}*/)
        {
            InitializeComponent();
//{[{
            DataContext = viewModel;
//}]}
        }
    }
}
