using ItemNamespace.ViewModel;
using Microsoft.Practices.ServiceLocation;
namespace uct.ItemName.View
{
    public sealed partial class ShellView : Page
    {
        private ShellViewModel ViewModel { get { return DataContext as ShellViewModel; } }

        private NavigationService navigationService { get { return ServiceLocator.Current.GetInstance<NavigationService>(); } }

        public ShellView()
        {
            this.InitializeComponent();

            navigationService.SetNavigationFrame(frame);
        }
    }
}