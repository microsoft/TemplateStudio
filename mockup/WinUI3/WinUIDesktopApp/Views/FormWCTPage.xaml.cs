using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

using WinUIDesktopApp.ViewModels;

namespace WinUIDesktopApp.Views
{
    public sealed partial class FormWCTPage : Page
    {
        public FormWCTViewModel ViewModel { get; }

        public FormWCTPage()
        {
            ViewModel = Ioc.Default.GetService<FormWCTViewModel>();
            InitializeComponent();
        }
    }
}
