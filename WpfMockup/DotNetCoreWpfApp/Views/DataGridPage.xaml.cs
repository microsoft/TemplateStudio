using System.Windows.Controls;

using DotNetCoreWpfApp.ViewModels;

namespace DotNetCoreWpfApp.Views
{
    public partial class DataGridPage : Page
    {
        public DataGridPage(DataGridViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
