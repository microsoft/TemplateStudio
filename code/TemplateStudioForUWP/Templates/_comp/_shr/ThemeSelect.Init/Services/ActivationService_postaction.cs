//{**
// These code blocks add the ThemeSelectorService initialization to the ActivationService of your project.
//**}
namespace Param_RootNamespace.Services
{
    internal class ActivationService
    {
        private async Task InitializeAsync()
        {
//^^
//{[{
            await ThemeSelectorService.InitializeAsync().ConfigureAwait(false);
//}]}
        }

        private async Task StartupAsync()
        {
//{[{
            await ThemeSelectorService.SetRequestedThemeAsync();
//}]}
        }
    }
}
