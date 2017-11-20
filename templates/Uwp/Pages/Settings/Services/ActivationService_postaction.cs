//{**
// These code blocks add the ThemeSelectorService initialization to the ActivationService of your project.
//**}

private async Task InitializeAsync()
{
    //{[{
    await ThemeSelectorService.InitializeAsync();
    //}]}
}

private async Task StartupAsync()
{
    //{[{
    ThemeSelectorService.SetRequestedTheme();
    //}]}
}
