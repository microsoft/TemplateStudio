namespace Param_RootNamespace.Services
{
    public class ActivationService : IActivationService
    {
        private readonly INavigationService _navigationService;
//{[{
        private readonly IThemeSelectorService _themeSelectorService;
//}]}
        public ActivationService(/*{[{*/IThemeSelectorService themeSelectorService/*}]}*/)
        {
//^^
//{[{
            _themeSelectorService = themeSelectorService;
//}]}
        }

        private async Task InitializeAsync()
        {
//{[{
            await _themeSelectorService.InitializeAsync().ConfigureAwait(false);
//}]}
        }

        private async Task StartupAsync()
        {
//^^
//{[{
            await _themeSelectorService.SetRequestedThemeAsync();
//}]}
            await Task.CompletedTask;
        }
    }
}
