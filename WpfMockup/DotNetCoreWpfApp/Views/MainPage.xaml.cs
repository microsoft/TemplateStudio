using System.Windows.Controls;

using DotNetCoreWpfApp.ViewModels;

namespace DotNetCoreWpfApp.Views
{
    public partial class MainPage : Page
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
