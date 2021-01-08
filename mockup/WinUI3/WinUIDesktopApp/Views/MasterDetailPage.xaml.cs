using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

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

        private void OnViewStateChanged(object sender, MasterDetailsViewState e)
        {
            if (e == MasterDetailsViewState.Both)
            {
                ViewModel.EnsureItemSelected();
            }
        }
    }
}
