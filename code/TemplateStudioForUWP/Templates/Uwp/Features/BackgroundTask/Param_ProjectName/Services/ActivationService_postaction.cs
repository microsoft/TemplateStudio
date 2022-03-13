//{**
// These code blocks include the BackgroundTaskService Instance in the method `GetActivationHandlers()`
// and background task registration to the method `InitializeAsync()` in the ActivationService of your project.
//**}

using System;
//{[{
using Param_RootNamespace.Core.Helpers;
//}]}

namespace Param_RootNamespace.Services
{
    internal class ActivationService
    {
        private async Task InitializeAsync()
        {
//{[{
            await Singleton<BackgroundTaskService>.Instance.RegisterBackgroundTasksAsync().ConfigureAwait(false);
//}]}
        }

        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
//{[{
            yield return Singleton<BackgroundTaskService>.Instance;
//}]}
//{--{
            yield break;
//}--}
        }
    }
}
