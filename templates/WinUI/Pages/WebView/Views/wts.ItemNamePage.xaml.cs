using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page
    {
        public wts.ItemNameViewModel ViewModel { get; }

        public wts.ItemNamePage()
        {
            ViewModel = Ioc.Default.GetService<wts.ItemNameViewModel>();
            InitializeComponent();
            ViewModel.WebViewService.Initialize(webView);
        }
    }
}