namespace Param_RootNamespace.Services
{
    public class ApplicationHostService : IHostedService
    {
        private readonly INavigationService _navigationService;
//{[{
        private readonly IToastNotificationsService _toastNotificationsService;
//}]}
        public ApplicationHostService(/*{[{*/IToastNotificationsService toastNotificationsService/*}]}*/)
        {
//^^
//{[{
            _toastNotificationsService = toastNotificationsService;
//}]}
        }

        private async Task StartupAsync()
        {
            if (!_isInitialized)
            {
//^^
//{[{
                _toastNotificationsService.ShowToastNotificationSample();
//}]}
                await Task.CompletedTask;
            }
        }
    }
}
