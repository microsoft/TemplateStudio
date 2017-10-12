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
            Container.RegisterInstance<IWhatsNewDisplayService>(new WhatsNewDisplayService());
//}]}
        }

        private async Task LaunchApplication(string page, object launchParam)
        {
            NavigationService.Navigate(page, launchParam);            
            Window.Current.Activate();
//{[{
            await Container.Resolve<IWhatsNewDisplayService>().ShowIfAppropriateAsync();
//}]}
        }
    }
}
