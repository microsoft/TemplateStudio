using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using WinUIDesktopApp.ViewModels;

namespace WinUIDesktopApp.Views
{
    public sealed partial class DataGridPage : Page
    {
        public DataGridViewModel ViewModel { get; }

        public DataGridPage()
        {
            //Microsoft.Toolkit.Uwp.UI.Controls.DataGrid
            ViewModel = Ioc.Default.GetService<DataGridViewModel>();
            InitializeComponent();
        }
    }
}
