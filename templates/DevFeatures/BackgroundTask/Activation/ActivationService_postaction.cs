private async Task InitializeAsync()
{
    Singleton<BackgroundTask.BackgroundTaskService>.Instance.RegisterBackgroundTasks();
}
private IEnumerable<ActivationHandler> GetActivationHandlers()
{
    yield return Singleton<BackgroundTask.BackgroundTaskService>.Instance;
}