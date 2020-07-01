using System;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Helpers;

namespace Param_RootNamespace.Core.Services
{
    public class IdentityService : IIdentityService
    {
        /*
        For more information about using Identity, see
        https://github.com/microsoft/WindowsTemplateStudio/blob/release/docs/WPF/services/identity.md

        Read more about Microsoft Identity Client here
        https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki
        https://docs.microsoft.com/azure/active-directory/develop/v2-overview

        TODO WTS: Please create a ClientID following these steps and update the appsettings.json IdentityClientId.
        https://docs.microsoft.com/azure/active-directory/develop/quickstart-register-app

        The provided clientID requests permissions on user.read, this might be blocked in environments that require admin consent.
        For more info about admin consent please see https://docs.microsoft.com/azure/active-directory/develop/application-consent-experience
        For more info creating protected APIs, see https://docs.microsoft.com/azure/active-directory/develop/scenario-protected-web-api-overview
        For more info on desktop apps that call protected APIs, see https://docs.microsoft.com/azure/active-directory/develop/scenario-desktop-overview
        */
        private readonly string[] _graphScopes = new string[] { "user.read" };
        private readonly IIdentityCacheService _identityCacheService;
        private bool _integratedAuthAvailable;
        private IPublicClientApplication _client;
        private AuthenticationResult _authenticationResult;

        public event EventHandler LoggedIn;

        public event EventHandler LoggedOut;

        public IdentityService(IIdentityCacheService identityCacheService)
        {
            _identityCacheService = identityCacheService;
        }

        public void InitializeWithAadAndPersonalMsAccounts(string clientId, string redirectUri = null)
        {
            _integratedAuthAvailable = false;
            _client = PublicClientApplicationBuilder.Create(clientId)
                                                    .WithAuthority(AadAuthorityAudience.AzureAdAndPersonalMicrosoftAccount)
                                                    .WithRedirectUri(redirectUri)
                                                    .Build();

            ConfigureCache();
        }

        public void InitializeWithAadMultipleOrgs(string clientId, bool integratedAuth = false, string redirectUri = null)
        {
            _integratedAuthAvailable = integratedAuth;
            _client = PublicClientApplicationBuilder.Create(clientId)
                                                    .WithAuthority(AadAuthorityAudience.AzureAdMultipleOrgs)
                                                    .WithRedirectUri(redirectUri)
                                                    .Build();

            ConfigureCache();
        }

        public void InitializeWithAadSingleOrg(string clientId, string tenant, bool integratedAuth = false, string redirectUri = null)
        {
            _integratedAuthAvailable = integratedAuth;
            _client = PublicClientApplicationBuilder.Create(clientId)
                                                    .WithAuthority(AzureCloudInstance.AzurePublic, tenant)
                                                    .WithRedirectUri(redirectUri)
                                                    .Build();

            ConfigureCache();
        }

        public bool IsLoggedIn() => _authenticationResult != null;

        public async Task<LoginResultType> LoginAsync()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                return LoginResultType.NoNetworkAvailable;
            }

            try
            {
                var accounts = await _client.GetAccountsAsync();
                _authenticationResult = await _client.AcquireTokenInteractive(_graphScopes)
                                                     .WithAccount(accounts.FirstOrDefault())
                                                     .ExecuteAsync();
                if (!IsAuthorized())
                {
                    _authenticationResult = null;
                    return LoginResultType.Unauthorized;
                }

                LoggedIn?.Invoke(this, EventArgs.Empty);
                return LoginResultType.Success;
            }
            catch (MsalClientException ex)
            {
                if (ex.ErrorCode == "authentication_canceled")
                {
                    return LoginResultType.CancelledByUser;
                }

                return LoginResultType.UnknownError;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return LoginResultType.UnknownError;
            }
        }

        public bool IsAuthorized()
        {
            // TODO WTS: You can also add extra authorization checks here.
            // i.e.: Checks permisions of _authenticationResult.Account.Username in a database.
            return true;
        }

        public string GetAccountUserName()
        {
            return _authenticationResult?.Account?.Username;
        }

        public async Task LogoutAsync()
        {
            try
            {
                var accounts = await _client.GetAccountsAsync();
                var account = accounts.FirstOrDefault();
                if (account != null)
                {
                    await _client.RemoveAsync(account);
                }

                _authenticationResult = null;
                LoggedOut?.Invoke(this, EventArgs.Empty);
            }
            catch (MsalException)
            {
                // TODO WTS: LogoutAsync can fail please handle exceptions as appropriate to your scenario
                // For more info on MsalExceptions see
                // https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/exceptions
            }
        }

        public async Task<string> GetAccessTokenForGraphAsync() => await GetAccessTokenAsync(_graphScopes);

        // All sensitive data in your app should be retrieven using access tokens.
        // This method provides you with an access token to secure calls to the Microsoft Graph or other protected API.
        // For more info on protecting web api with tokens see https://docs.microsoft.com/azure/active-directory/develop/scenario-protected-web-api-overview
        public async Task<string> GetAccessTokenAsync(string[] scopes)
        {
            var acquireTokenSuccess = await AcquireTokenSilentAsync(scopes);
            if (acquireTokenSuccess)
            {
                return _authenticationResult.AccessToken;
            }
            else
            {
                try
                {
                    // Interactive authentication is required
                    var accounts = await _client.GetAccountsAsync();
                    _authenticationResult = await _client.AcquireTokenInteractive(scopes)
                                                         .WithAccount(accounts.FirstOrDefault())
                                                         .ExecuteAsync();
                    return _authenticationResult.AccessToken;
                }
                catch (MsalException)
                {
                    // AcquireTokenSilent and AcquireTokenInteractive failed, the session will be closed.
                    _authenticationResult = null;
                    LoggedOut?.Invoke(this, EventArgs.Empty);
                    return string.Empty;
                }
            }
        }

        public async Task<bool> AcquireTokenSilentAsync() => await AcquireTokenSilentAsync(_graphScopes);

        private async Task<bool> AcquireTokenSilentAsync(string[] scopes)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                return false;
            }

            try
            {
                var accounts = await _client.GetAccountsAsync();
                _authenticationResult = await _client.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                                                     .ExecuteAsync();
                return true;
            }
            catch (MsalUiRequiredException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                if (_integratedAuthAvailable)
                {
                    try
                    {
                        _authenticationResult = await _client.AcquireTokenByIntegratedWindowsAuth(_graphScopes)
                                                             .ExecuteAsync();
                        return true;
                    }
                    catch (MsalUiRequiredException)
                    {
                        // Interactive authentication is required
                        return false;
                    }
                }
                else
                {
                    // Interactive authentication is required
                    return false;
                }
            }
            catch (MsalException)
            {
                // TODO WTS: Silentauth failed, please handle this exception as appropriate to your scenario
                // For more info on MsalExceptions see
                // https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/exceptions
                return false;
            }
        }

        private void ConfigureCache()
        {
            if (_identityCacheService != null)
            {
                // .NET core applications should provide a mechanism to serialize and deserialize the user token cache
                // https://aka.ms/msal-net-token-cache-serialization
                _client.UserTokenCache.SetBeforeAccess((args) =>
                {
                    var data = _identityCacheService.ReadMsalToken();
                    if (data != default)
                    {
                        args.TokenCache.DeserializeMsalV3(data);
                    }
                });
                _client.UserTokenCache.SetAfterAccess((args) =>
                {
                    if (args.HasStateChanged)
                    {
                        _identityCacheService.SaveMsalToken(args.TokenCache.SerializeMsalV3());
                    }
                });
            }
        }
    }
}
