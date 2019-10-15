namespace Param_RootNamespace.Services
{
    internal class ApplicationHostService : IHostedService
    {
        private readonly INavigationService _navigationService;
//{[{
        private readonly IPersistAndRestoreService _persistAndRestoreService;
//}]}

        public ApplicationHostService(INavigationService navigationService, IShellPage shellPage, IThemeSelectorService themeSelectorService)
        {
            _navigationService = navigationService;
//{[{
            _persistAndRestoreService = persistAndRestoreService;
//}]}
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
//{[{
            _persistAndRestoreService.PersistData();
//}]}
        }

        private async Task InitializeAsync()
        {
            await Task.CompletedTask;
//{[{
            _persistAndRestoreService.RestoreData();
//}]}
        }
    }
}
