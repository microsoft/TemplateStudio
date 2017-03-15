using Microsoft.Practices.ServiceLocation;
namespace uct.ItemName.Shell
{
    public sealed partial class ShellPage : Page
    {
        private ShellViewModel ViewModel { get { return DataContext as ShellViewModel; } }

        private NavigationService navigationService { get { return ServiceLocator.Current.GetInstance<NavigationService>(); } }

        public ShellPage()
        {
            this.InitializeComponent();

            navigationService.SetNavigationFrame(frame);
        }
    }
}