//{[{
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Services;
//}]}
namespace Param_RootNamespace
{
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // App Services
//{[{
            containerRegistry.RegisterSingleton<IRightPaneService, RightPaneService>();
//}]}
        }
    }
}