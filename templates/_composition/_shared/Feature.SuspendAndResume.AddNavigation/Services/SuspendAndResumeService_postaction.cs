using System.Reflection;
using Windows.UI.Xaml.Controls;
namespace Param_ItemNamespace.Services
{
    internal class SuspendAndResumeService : ActivationHandler<LaunchActivatedEventArgs>
    {
        private async Task RestoreStateAsync()
        {
            //^^
            if (typeof(Page).IsAssignableFrom(saveState?.Target))
            {
                NavigationService.Navigate(saveState.Target, saveState.SuspensionState);
            }
        }
    }
}
