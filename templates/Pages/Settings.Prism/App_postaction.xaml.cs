//{[{
using Prism.Mvvm;
using System.Globalization;
using Param_RootNamespace.Services;
//}]}
namespace Param_RootNamespace
{
    public sealed partial class App : PrismUnityApplication
    {

        private async Task LaunchApplicationAsync(string page, object launchParam)
        {
//{[{
            Services.ThemeSelectorService.SetRequestedTheme();
//}]}
        }

        protected override Task OnActivateApplicationAsync(IActivatedEventArgs args)
        {
            return Task.CompletedTask;
        }

        protected override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
//{[{
            await ThemeSelectorService.InitializeAsync().ConfigureAwait(false);
//}]}
            });
            await base.OnInitializeAsync(args);
        }
    }
}
