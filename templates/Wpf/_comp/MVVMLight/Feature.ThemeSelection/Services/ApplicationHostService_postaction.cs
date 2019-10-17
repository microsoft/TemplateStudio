namespace Param_RootNamespace.Services
{
    public class ApplicationHostService : IApplicationHostService
    {
        private readonly INavigationService _navigationService;
//{[{
        private readonly IThemeSelectorService _themeSelectorService;
//}]}

        public ApplicationHostService(/*{[{*/IThemeSelectorService themeSelectorService/*}]}*/)
        {
//^^
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
