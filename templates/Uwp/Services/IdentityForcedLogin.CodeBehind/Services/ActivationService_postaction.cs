//{[{
using Param_RootNamespace.Core.Helpers;
using Param_RootNamespace.Core.Services;
//}]}

namespace Param_RootNamespace.Services
{
    internal class ActivationService
    {
        private object _lastActivationArgs;
//{[{

        private IdentityService IdentityService => Singleton<IdentityService>.Instance;

        private UserDataService UserDataService => Singleton<UserDataService>.Instance;
//}]}
        public ActivationService(App app, Type defaultNavItem, Lazy<UIElement> shell = null)
        {
//^^
//{[{
            IdentityService.LoggedIn += OnLoggedIn;
//}]}
        }

        public async Task ActivateAsync(object activationArgs)
        {
            if (IsInteractive(activationArgs))
            {
                await InitializeAsync();
//{[{
                UserDataService.Initialize();
                IdentityService.InitializeWithAadAndPersonalMsAccounts();
                var silentLoginSuccess = await IdentityService.AcquireTokenSilentAsync();
                if (!silentLoginSuccess || !IdentityService.IsAuthorized())
                {
                    await RedirectLoginPageAsync();
                }
//}]}
            }
//{--{
            await HandleActivationAsync(activationArgs);
//}--}
//^^
//{[{
            if (IdentityService.IsLoggedIn())
            {
                await HandleActivationAsync(activationArgs);
            }
//}]}

            _lastActivationArgs = activationArgs;
        }
//{[{

        private async void OnLoggedIn(object sender, EventArgs e)
        {
            if (_shell?.Value != null)
            {
                Window.Current.Content = _shell.Value;
            }
            else
            {
                var frame = new Frame();
                Window.Current.Content = frame;
                NavigationService.Frame = frame;
            }

            await ThemeSelectorService.SetRequestedThemeAsync();
            await HandleActivationAsync(_lastActivationArgs);
        }

        public async Task RedirectLoginPageAsync()
        {
            var frame = new Frame();
            NavigationService.Frame = frame;
            Window.Current.Content = frame;
            await ThemeSelectorService.SetRequestedThemeAsync();
            NavigationService.Navigate<Views.LogInPage>();
        }
//}]}
    }
}
