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
            yield return Singleton<wts.ItemNameActivationHandler>.Instance;
//}]}
//{--{
            yield break;
//}--}
        }

        //^^
        //{[{
        internal async Task ActivateFromShareTargetAsync(ShareTargetActivatedEventArgs activationArgs)
        {
            var shareTargetHandler = GetActivationHandlers().FirstOrDefault(h => h.CanHandle(activationArgs));
            if (shareTargetHandler != null)
            {
                await shareTargetHandler.HandleAsync(activationArgs);
            }
        }
        //}]}
    }
}
