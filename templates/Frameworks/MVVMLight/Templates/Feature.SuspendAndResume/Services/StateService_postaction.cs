using Microsoft.Practices.ServiceLocation;
namespace ItemNamespace.Services
{
    public class StateService
    {
        public async Task RestoreStateAsync(ApplicationExecutionState previousState, string arguments)
        {
            if (previousState == ApplicationExecutionState.Terminated)
            {
                if (saveState != null && saveState.Page != null)
                {
                    //Navigate to page
                    var navigationService = ServiceLocator.Current.GetInstance<NavigationService>();
                    navigationService.Navigate(navigationService.GetViewModel(saveState.Page));
                    //Restore page state
                    RestoreState?.Invoke(this, new RestoreStateEventArgs(saveState.PageState));
                }  
            }
        }
    }
}
