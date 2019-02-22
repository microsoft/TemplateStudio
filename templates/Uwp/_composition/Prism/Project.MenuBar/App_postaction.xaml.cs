using Windows.UI.Xaml;
//^^
//{[{
using Windows.UI.Xaml.Controls;
using Prism.Windows.Navigation;
using Param_RootNamespace.Views;
//}]}

namespace Param_RootNamespace
{
    public sealed partial class App : PrismUnityApplication
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void ConfigureContainer()
        {
//^^
//{[{
            Container.RegisterType<IMenuNavigationService, MenuNavigationService>();
//}]}
        }

        private async Task LaunchApplicationAsync(string page, object launchParam)
        {
//^^
//{[{
            var menuNavigationService = Container.Resolve<IMenuNavigationService>();
            menuNavigationService.UpdateView(page, launchParam);
//}]}
            Window.Current.Activate();
        }
//^^
//{[{

        public void SetNavigationFrame(Frame frame)
        {
            var sessionStateService = Container.Resolve<ISessionStateService>();
            CreateNavigationService(new FrameFacadeAdapter(frame), sessionStateService);
        }

        protected override UIElement CreateShell(Frame rootFrame)
        {
            var shell = Container.Resolve<ShellPage>();
            shell.SetRootFrame(rootFrame);
            return shell;
        }
//}]}
    }
}
