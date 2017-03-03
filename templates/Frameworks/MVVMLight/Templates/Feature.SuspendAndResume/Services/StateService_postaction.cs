using Microsoft.Practices.ServiceLocation;

namespace ItemNamespace.Services
{
    public class StateService
    {
        private void NavigateToPage(Type page, string arguments)
        {
            var navigationService = ServiceLocator.Current.GetInstance<NavigationService>();
            navigationService.Navigate(navigationService.GetViewModel(page));
        }
    }
}
