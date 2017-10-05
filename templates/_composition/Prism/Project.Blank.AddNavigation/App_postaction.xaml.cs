namespace Param_RootNamespace
{
    public sealed partial class App : PrismUnityApplication
    {
//^^
//{[{
    
        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            NavigationService.Navigate(PageTokens.Param_HomeNamePage, null);
            return Task.FromResult(true);
        }
//}]}
    }
}
