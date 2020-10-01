using System.Windows.Controls;

using DotNetCoreWpfApp.ViewModels;

namespace DotNetCoreWpfApp.Views
{
    public partial class MasterDetailPage : Page
    {
        public MasterDetailPage(MasterDetailViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
