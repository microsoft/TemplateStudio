using Microsoft.Practices.ServiceLocation;
namespace ItemNamespace.Services
{
    public class SuspendAndResumeService
    {
        public async Task RestoreStateAsync()
        {          
            if (typeof(Page).IsAssignableFrom(saveState?.Page))
            {
                //Navigate to page
                var navigationService = ServiceLocator.Current.GetInstance<NavigationService>();
                navigationService.Navigate(navigationService.GetViewModel(saveState.Page), saveState.SuspensionState);
            }
        }
    }
}
