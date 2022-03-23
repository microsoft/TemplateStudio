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
            yield return Singleton<SchemeActivationHandler>.Instance;
            //}]}
//{--{
            yield break;
//}--}
        }
    }
}
