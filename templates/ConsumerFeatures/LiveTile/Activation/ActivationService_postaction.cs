private async Task InitializeAsync()
{
    Services.LiveTileService.EnableQueue();
}
private async Task StartupAsync()
{
    Services.LiveTileService.SampleUpdate();
}