//{[{
using Prism.Mvvm;
using System.Globalization;
using Param_RootNamespace.Services;
//}]}
namespace Param_RootNamespace
{
    public sealed partial class App : PrismUnityApplication
    {
        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
//{[{
            Services.ThemeSelectorService.SetRequestedTheme();
//}]}
        }

//{--{
        protected override Task OnInitializeAsync(IActivatedEventArgs args)//}--}
//{[{
        protected async override Task OnInitializeAsync(IActivatedEventArgs args)
//}]}
        {
//{[{
            await ThemeSelectorService.InitializeAsync();
//}]}
            });
//{--{
            return base.OnInitializeAsync(args);//}--}
//{[{
            await base.OnInitializeAsync(args);
//}]}
        }
    }
}
