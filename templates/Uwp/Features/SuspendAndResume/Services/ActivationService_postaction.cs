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
