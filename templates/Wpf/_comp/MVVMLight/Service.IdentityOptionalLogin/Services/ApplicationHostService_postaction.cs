//{[{
using Param_RootNamespace.Models;
using Param_RootNamespace.Core.Contracts.Services;
using Microsoft.Identity.Client.Extensions.Msal;
//}]}

namespace Param_RootNamespace.Services
{
    public class ApplicationHostService : IApplicationHostService
    {
        private readonly IThemeSelectorService _themeSelectorService;
//{[{
        private readonly IIdentityService _identityService;
        private readonly IUserDataService _userDataService;
        private readonly AppConfig _config;
//}]}
        public ApplicationHostService(/*{[{*/IIdentityService identityService, IUserDataService userDataService, AppConfig config/*}]}*/)
        {
//^^
//{[{
            _identityService = identityService;
            _userDataService = userDataService;
            _config = config;
//}]}
        }

        public async Task StartAsync()
        {
            await InitializeAsync();
//{[{

            // https://aka.ms/msal-net-token-cache-serialization
            var storageCreationProperties = new StorageCreationPropertiesBuilder(_config.IdentityCacheFileName, _config.IdentityCacheDirectoryName, _config.IdentityClientId).Build();
            var cacheHelper = await MsalCacheHelper.CreateAsync(storageCreationProperties).ConfigureAwait(false);
            _identityService.InitializeWithAadAndPersonalMsAccounts(_config.IdentityClientId, "http://localhost", cacheHelper);
            await _identityService.AcquireTokenSilentAsync();
//}]}
        }

        private async Task InitializeAsync()
        {
//^^
//{[{
            _userDataService.Initialize();
//}]}
        }
    }
}
