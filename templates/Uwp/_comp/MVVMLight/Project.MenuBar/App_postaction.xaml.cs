namespace Param_RootNamespace
{
    public sealed partial class App : Application
    {
//^^
//{[{

        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, typeof(ViewModels.Param_HomeNameViewModel), new Lazy<UIElement>(CreateShell));
        }

        private UIElement CreateShell()
        {
            return new Views.ShellPage();
        }
//}]}
    }
}
