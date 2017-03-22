private async Task InitializeAsync()
{
    await Singleton<Services.LiveTileService>.Instance.EnableQueueAsync();
}
private async Task StartupAsync()
{
    Singleton<Services.LiveTileService>.Instance.SampleUpdate();
}
private IEnumerable<ActivationHandler> GetActivationHandlers()
{
    yield return Singleton<Services.LiveTileService>.Instance;
}