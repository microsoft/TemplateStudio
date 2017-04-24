using Microsoft.Practices.ServiceLocation;
namespace Param_ItemNamespace.Services
{
    internal class SuspendAndResumeService : ActivationHandler<LaunchActivatedEventArgs>
    {
        private async Task RestoreStateAsync()
        {
            //^^
            var navigationService = ServiceLocator.Current.GetInstance<NavigationServiceEx>();
            navigationService.Navigate(saveState.Target.FullName, saveState.SuspensionState);
        }
    }
}