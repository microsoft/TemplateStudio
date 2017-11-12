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
            Container.RegisterInstance<IFirstRunDisplayService>(new FirstRunDisplayService());
//}]}
        }

        private async Task LaunchApplicationAsync(string page, object launchParam)
        {
            NavigationService.Navigate(page, launchParam);
            Window.Current.Activate();
//{[{
            await Container.Resolve<IFirstRunDisplayService>().ShowIfAppropriateAsync();
//}]}
        }
    }
}
