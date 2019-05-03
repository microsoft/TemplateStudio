//{**
//This code block includes the SuspendAndResumeService Instance in the method
//`GetActivationHandlers()` in the ActivationService of your project.
//**}

using System;
//{[{
using Param_RootNamespace.Core.Helpers;
//}]}

namespace Param_RootNamespace.Services
{
    internal class ActivationService
    {
        public async Task ActivateAsync(object activationArgs)
        {
            if (IsInteractive(activationArgs))
            {
            }

            if (IsInteractive(activationArgs))
            {
//{[{
                var activation = activationArgs as IActivatedEventArgs;
                if (activation.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    await Singleton<SuspendAndResumeService>.Instance.RestoreSuspendAndResumeData();
                }

//}]}
            }
        }

        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
//{[{
            yield return Singleton<SuspendAndResumeService>.Instance;
//}]}
//{--{
            yield break;
//}--}
        }
    }
}
