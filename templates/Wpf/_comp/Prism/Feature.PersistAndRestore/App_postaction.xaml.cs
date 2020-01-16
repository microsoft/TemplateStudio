//{[{
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Services;
//}]}
namespace Param_RootNamespace
{
    public partial class App : PrismApplication
    {
        protected async override void InitializeShell(Window shell)
        {
            base.InitializeShell(shell);
//{[{
            var persistAndRestoreService = Container.Resolve<IPersistAndRestoreService>();
            persistAndRestoreService.RestoreData();
//}]}
        }

        protected async override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // App Services
//{[{
            containerRegistry.Register<IPersistAndRestoreService, PersistAndRestoreService>();
//}]}
        }

        private async void OnExit(object sender, ExitEventArgs e)
        {
//^^
//{[{
            var persistAndRestoreService = Container.Resolve<IPersistAndRestoreService>();
            persistAndRestoreService.PersistData();
//}]}
        }
    }
}
