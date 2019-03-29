//{[{
using Param_RootNamespace.ViewModels;
//}]}

namespace Param_RootNamespace
{
    public sealed partial class App
    {
        private UIElement CreateShell()
        {
            var shellPage = new Views.ShellPage();
            //{[{
            _container.RegisterInstance(typeof(IConnectedAnimationService), nameof(IConnectedAnimationService), new ConnectedAnimationService(shellPage.GetFrame()));
            //}]}
            return shellPage;
        }
    }
}
