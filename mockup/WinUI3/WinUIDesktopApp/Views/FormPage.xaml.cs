using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using WinUIDesktopApp.ViewModels;

namespace WinUIDesktopApp.Views
{
    public sealed partial class FormPage : Page
    {
        public FormViewModel ViewModel { get; }

        public FormPage()
        {
            ViewModel = Ioc.Default.GetService<FormViewModel>();
            InitializeComponent();
        }
    }
}
