using wts.ItemName.ViewModels;
using Caliburn.Micro;
using Microsoft.UI.Xaml.Controls;

namespace wts.ItemName.Views
{
    // TODO WTS: Change the icons and titles for all NavigationViewItems in ShellPage.xaml.
    public sealed partial class ShellPage : IShellView
    {
        private ShellViewModel ViewModel => DataContext as ShellViewModel;

        public ShellPage()
        {
            InitializeComponent();
        }

        public INavigationService CreateNavigationService(WinRTContainer container)
        {
            var navigationService = container.RegisterNavigationService(shellFrame);
            navigationViewHeaderBehavior.Initialize(navigationService);
            return navigationService;
        }

        public NavigationView GetNavigationView()
        {
            return navigationView;
        }
    }
}
