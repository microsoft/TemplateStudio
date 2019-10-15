namespace Param_RootNamespace.Services
{
    public class ApplicationHostService : IApplicationHostService
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
