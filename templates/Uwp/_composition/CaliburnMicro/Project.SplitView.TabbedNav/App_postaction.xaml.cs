//{[{
using Param_RootNamespace.ViewModels;
//}]}

namespace Param_RootNamespace
{
    public sealed partial class App
    {
        protected override void Configure()
        {
            //^^
            //{[{
            _container.PerRequest<ShellViewModel>();
            //}]}
        }
//^^
//{[{

        private ActivationService CreateActivationService()
        {
            return new ActivationService(_container, typeof(ViewModels.Param_HomeNameViewModel), new Lazy<UIElement>(CreateShell));
        }

        private UIElement CreateShell()
        {
            var shellPage = new Views.ShellPage();
            return shellPage;
        }
//}]}
    }
}
