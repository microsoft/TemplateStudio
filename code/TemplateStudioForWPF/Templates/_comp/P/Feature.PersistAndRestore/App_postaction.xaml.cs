//{[{
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Services;
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Services;
//}]}
namespace Param_RootNamespace;

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
        // Core Services
//{[{
        containerRegistry.Register<IFileService, FileService>();
//}]}
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
