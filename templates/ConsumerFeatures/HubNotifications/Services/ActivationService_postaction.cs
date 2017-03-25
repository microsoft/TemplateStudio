private async Task StartupAsync()
{
    //Singleton<Services.HubNotificationsService>.Instance.InitializeAsync();
}

private IEnumerable<ActivationHandler> GetActivationHandlers()
{
    yield return Singleton<Services.HubNotificationsService>.Instance;
}