//{**
// These code blocks add the WindowManagerService initialization to the ActivationService of your project.
//**}

private async Task InitializeAsync()
{
//^^
//{[{
    await WindowManagerService.Current.InitializeAsync();
//}]}
}
