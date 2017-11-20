//{**
// This code block include the ToastNotificationsFeatureService Instance in the method 
// `GetActivationHandlers()` in the ActivationService of your project.
//**}

using System;
//{[{
using Param_RootNamespace.Helpers;
using Param_RootNamespace.Services;
//}]}

namespace Param_ItemNamespace.Services
{
    internal class ActivationService
    {
        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
            //{[{
            yield return Singleton<ToastNotificationsFeatureService>.Instance;
            //}]}
//{--{
            yield break;//}--}
        }
    }
}
