//{[{
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Services;
//}]}
namespace Param_RootNamespace
{
    public partial class App : PrismApplication
    {
        protected override async void OnInitialized()
        {
//{[{
            var persistAndRestoreService = Container.Resolve<IPersistAndRestoreService>();
            persistAndRestoreService.RestoreData();

//}]}
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // App Services
//{[{
            containerRegistry.Register<IPersistAndRestoreService, PersistAndRestoreService>();
//}]}
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
//^^
//{[{
            var persistAndRestoreService = Container.Resolve<IPersistAndRestoreService>();
            persistAndRestoreService.PersistData();
//}]}
        }
    }
}
