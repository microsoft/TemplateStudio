using Caliburn.Micro;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using wts.ItemName.Services;
using wts.ItemName.ViewModels;

namespace wts.ItemName.Views
{
    public sealed partial class ShellPage : IShellView
    {
        private ShellViewModel ViewModel => DataContext as ShellViewModel;

        public ShellPage()
        {
            InitializeComponent();
            HideNavViewBackButton();
        }

        public INavigationService CreateNavigationService(WinRTContainer container)
        {
            return container.RegisterNavigationService(shellFrame);
        }

        public NavigationView GetNavigationView()
        {
            return navigationView;
        }

        private void HideNavViewBackButton()
        {
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 6))
            {
                navigationView.IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed;
            }
        }
    }
}
