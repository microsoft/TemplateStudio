using Windows.UI.Xaml;
//^^
//{[{
using Param_RootNamespace.Services;
//}]}

namespace Param_RootNamespace
{
    public sealed partial class App : PrismUnityApplication
    {
        public App()
        {
            InitializeComponent();
        }

//{--{
            await Task.CompletedTask;//}--}

        private async Task LaunchApplication(string page, object launchParam)
        {
            NavigationService.Navigate(page, launchParam);            
//{[{
            Window.Current.Activate();
            await WhatsNewDisplayService.ShowIfAppropriateAsync();
//}]}
        }
    }
}
