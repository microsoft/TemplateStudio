namespace Param_RootNamespace.Views
{
    public sealed partial class ShellPage : IShellView
    {
        public INavigationService CreateNavigationService(WinRTContainer container)
        {
            var navigationService = container.RegisterNavigationService(shellFrame);
//{[{
            navigationViewHeaderBehavior.Initialize(navigationService);
//}]}
            return navigationService;
        }
    }
}