//{[{
using Param_RootNamespace.Core.Services;
//}]}
namespace Param_RootNamespace
{
    public sealed partial class App : PrismUnityApplication
    {
        protected override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            await base.OnInitializeAsync(args);
//{[{
            var sampleDataService = Container.Resolve<ISampleDataService>();
            sampleDataService.Initialize("ms-appx:///Assets");
//}]}
        }
    }
}
