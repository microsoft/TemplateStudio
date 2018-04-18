//{[{
using Caliburn.Micro;
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
                var navigationService = IoC.Get<INavigationService>();
                navigationService.NavigateToViewModel(saveState.Target, saveState.SuspensionState);
            }
            //}]}
        }
    }
}
