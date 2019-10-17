//{[{
using Param_RootNamespace.ViewModels;
//}]}

namespace Param_RootNamespace.Views
{
    public partial class ShellWindow : MetroWindow, IShellWindow
    {
        public ShellWindow(/*{[{*/ShellViewModel viewModel/*}]}*/)
        {
            InitializeComponent();
//{[{
            DataContext = viewModel;
//}]}
        }
    }
}
