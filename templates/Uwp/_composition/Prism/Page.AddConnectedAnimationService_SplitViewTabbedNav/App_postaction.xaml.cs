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

        protected override UIElement CreateShell(Frame rootFrame)
        {
            var shell = Container.Resolve<ShellPage>();
            shell.SetRootFrame(rootFrame);
//^^
//{[{
            Container.RegisterInstance<IConnectedAnimationService>(new ConnectedAnimationService(rootFrame));
//}]}
            return shell;
        }

    }
}
