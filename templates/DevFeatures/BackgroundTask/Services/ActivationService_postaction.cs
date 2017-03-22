private async Task InitializeAsync()
{
    Singleton<Services.BackgroundTaskService>.Instance.RegisterBackgroundTasks();
}
private IEnumerable<ActivationHandler> GetActivationHandlers()
{
    yield return Singleton<Services.BackgroundTaskService>.Instance;
}