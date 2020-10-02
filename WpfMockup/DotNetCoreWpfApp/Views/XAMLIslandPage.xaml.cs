using System.Windows.Controls;

using DotNetCoreWpfApp.ViewModels;

namespace DotNetCoreWpfApp.Views
{
    public partial class XAMLIslandPage : Page
    {
        public XAMLIslandPage(XAMLIslandViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
