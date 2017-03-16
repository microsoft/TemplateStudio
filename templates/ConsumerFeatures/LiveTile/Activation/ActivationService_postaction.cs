private async Task InitializeAsync()
{
    Singleton<Services.LiveTileService>.Instance.EnableQueue();
}
private async Task StartupAsync()
{
    Singleton<Services.LiveTileService>.Instance.SampleUpdate();
}
private IEnumerable<ActivationHandler> GetActivationHandlers()
{
    yield return Singleton<Services.LiveTileService>.Instance;
}