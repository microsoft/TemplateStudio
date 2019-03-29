namespace Param_RootNamespace.Services
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
