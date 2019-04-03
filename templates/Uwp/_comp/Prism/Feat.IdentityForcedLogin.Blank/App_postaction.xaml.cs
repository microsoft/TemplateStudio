namespace Param_RootNamespace
{
    public sealed partial class App : PrismUnityApplication
    {
//{[{
        private LaunchActivatedEventArgs _lastActivationArgs;
//}]}

        protected override void ConfigureContainer()
        {
            var identityService = new IdentityService();
//{[{
            identityService.LoggedIn += OnLoggedIn;
            identityService.LoggedOut += OnLoggedOut;
//}]}
        }
//{[{

        private void OnLoggedIn(object sender, EventArgs e)
        {
            NavigationService.Navigate(PageTokens.Param_HomeNamePage, _lastActivationArgs);
            NavigationService.ClearHistory();
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            NavigationService.Navigate(PageTokens.LogInPage, null);
            NavigationService.ClearHistory();
        }
//}]}
        protected override async Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
//{[{
            var identityService = Container.Resolve<IIdentityService>();
            var silentLoginSuccess = await identityService.AcquireTokenSilentAsync();
            if (!silentLoginSuccess || !identityService.IsAuthorized())
            {
                _lastActivationArgs = args;
                await LaunchApplicationAsync(PageTokens.LogInPage, null);
                return;
            }

//}]}
        }
    }
}