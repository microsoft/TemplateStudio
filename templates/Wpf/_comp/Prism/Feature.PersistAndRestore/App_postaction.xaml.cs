//{[{
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Services;
//}]}
namespace Param_RootNamespace
{
    public partial class App : PrismApplication
    {
        public override void Initialize()
        {
            base.Initialize();
//{[{
            var persistAndRestoreService = Container.Resolve<IPersistAndRestoreService>();
            persistAndRestoreService.RestoreData();
//}]}
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
//^^
//{[{
            containerRegistry.Register<IPersistAndRestoreService, PersistAndRestoreService>();
//}]}
            // Views
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
