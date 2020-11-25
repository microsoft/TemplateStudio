//{[{
using Param_RootNamespace.ViewModels;
//}]}

namespace Param_RootNamespace.Views
{
        public ShellWindow(/*{[{*/ShellViewModel viewModel/*}]}*/)
        {
            InitializeComponent();
//{[{
            DataContext = viewModel;
//}]}
        }
}
