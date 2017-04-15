private async Task InitializeAsync()
{
    await ThemeSelectorService.InitializeAsync();
}

private async Task StartupAsync()
{
    Services.ThemeSelectorService.SetRequestedTheme();
}
