private async Task StartupAsync()
{
    Singleton<Services.ToastNotificationsService>.Instance.ShowToastNotificationSample();
}
private IEnumerable<ActivationHandler> GetActivationHandlers()
{
    yield return Singleton<Services.ToastNotificationsService>.Instance;
}