namespace Param_RootNamespace.Services
{
    internal class ApplicationHostService : IHostedService
    {
        private readonly INavigationService _navigationService;
//{[{
        private readonly IThemeSelectorService _themeSelectorService;
//}]}

        public ApplicationHostService(INavigationService navigationService, IShellPage shellPage)
        {
            _navigationService = navigationService;
//{[{
            _themeSelectorService = themeSelectorService;
//}]}
        }

        private async Task InitializeAsync()
        {
//^^
//{[{
            _themeSelectorService.SetTheme();
//}]}
        }
    }
}
