//{[{
using CommonServiceLocator;
//}]}
namespace Param_ItemNamespace.Services
{
    internal class SuspendAndResumeService : ActivationHandler<LaunchActivatedEventArgs>
    {
        private async Task RestoreStateAsync()
        {
            //^^
            //{[{
            if (saveState?.Target != null)
            {
                var navigationService = ServiceLocator.Current.GetInstance<NavigationServiceEx>();
                navigationService.Navigate(saveState.Target.FullName, saveState.SuspensionState);
            }
            //}]}
        }
    }
}
