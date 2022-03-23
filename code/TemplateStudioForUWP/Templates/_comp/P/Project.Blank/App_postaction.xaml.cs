namespace Param_RootNamespace
{
    public sealed partial class App : PrismUnityApplication
    {
        private async Task LaunchApplicationAsync(string page, object launchParam)
        {
//^^
//{[{
            NavigationService.Navigate(page, launchParam);
//}]}
            Window.Current.Activate();
        }
    }
}
