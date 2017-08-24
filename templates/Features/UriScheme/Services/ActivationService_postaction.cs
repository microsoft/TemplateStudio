using System;
//{[{
using Param_RootNamespace.Helpers;
//}]}

namespace Param_ItemNamespace.Services
{
    internal class ActivationService
    {
        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
            //{[{
            yield return Singleton<SchemeActivationHandler>.Instance;
            //}]}
//{--{

            yield break;//}--}
        }
    }
}
