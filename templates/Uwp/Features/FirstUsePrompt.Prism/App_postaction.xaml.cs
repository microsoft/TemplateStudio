using Windows.UI.Xaml;
//^^
//{[{
using Param_RootNamespace.Services;
//}]}

namespace Param_RootNamespace
{
    public sealed partial class App : PrismUnityApplication
    {
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
//{[{
            Container.RegisterType<IFirstRunDisplayService, FirstRunDisplayService>(new ContainerControlledLifetimeManager());
//}]}
        }

        private async Task LaunchApplicationAsync(string page, object launchParam)
        {
            Window.Current.Activate();
//{[{
            await Container.Resolve<IFirstRunDisplayService>().ShowIfAppropriateAsync();
//}]}
        }
    }
}
