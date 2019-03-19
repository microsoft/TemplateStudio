//{**
// These code blocks add the ThemeSelectorService initialization to the ActivationService of your project.
//**}
namespace Param_RootNamespace.Services
{
    internal class ActivationService
    {
        private async Task InitializeAsync()
        {
//{[{
            await ThemeSelectorService.InitializeAsync();
//}]}
        }

        private async Task StartupAsync()
        {
//{[{
            if (Window.Current.Content == null)
            {
                await ThemeSelectorService.InitializeAsync();
            }

//}]}
        }
    }
}