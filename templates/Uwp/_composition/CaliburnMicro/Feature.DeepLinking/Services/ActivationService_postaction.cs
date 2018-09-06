namespace Param_ItemNamespace.Services
{
    internal class ActivationService
    {
        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
//{[{
            yield return new SchemeActivationHandler(NavigationService);
//}]}
//{--{
            yield break;
//}--}
        }
    }
}
