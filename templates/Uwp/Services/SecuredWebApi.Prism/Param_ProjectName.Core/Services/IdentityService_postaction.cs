namespace Param_RootNamespace.Core.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly string[] _graphScopes = new string[] { "user.read" };

//{[{
        // TODO WTS: Follow these steps to configure access to the Web Api on your client application registration,
        // update the App.config's ResourceId and WebApiScope.
        // https://docs.microsoft.com/azure/active-directory/develop/quickstart-configure-app-access-web-apis#add-permissions-to-access-web-apis
        private readonly string[] _webApiScopes = { $"{ConfigurationManager.AppSettings["ResourceId"]}/{ConfigurationManager.AppSettings["WebApiScope"]}" };
//}]}
        public async Task<LoginResultType> LoginAsync()
        {
                _authenticationResult = await _client.AcquireTokenInteractive(_graphScopes)
//{[{
                                                         .WithExtraScopesToConsent(_webApiScopes)
//}]}
        }

        public async Task<string> GetAccessTokenForGraphAsync() => await GetAccessTokenAsync(_graphScopes);

//{[{
        public async Task<string> GetAccessTokenForWebApiAsync() => await GetAccessTokenAsync(_webApiScopes);
//}]}
}