using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using WinUIDesktopApp.ViewModels;

namespace WinUIDesktopApp.Views
{
    public sealed partial class MasterDetailPage : Page
    {
        public MasterDetailViewModel ViewModel { get; }

        public MasterDetailPage()
        {
            ViewModel = Ioc.Default.GetService<MasterDetailViewModel>();
            InitializeComponent();
        }
    }
}
