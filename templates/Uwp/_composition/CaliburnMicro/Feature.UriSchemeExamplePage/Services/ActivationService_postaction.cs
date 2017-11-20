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

            yield break;//}--}
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
