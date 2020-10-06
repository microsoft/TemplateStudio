namespace Param_RootNamespace.Services
{
    internal class ActivationService
    {
//^^
//{[{

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
