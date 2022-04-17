//{[{
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Services;
//}]}
namespace Param_RootNamespace.Tests.MSTest
{
    [TestClass]
    public class PagesTests
    {
        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            // Core Services
//{[{
            services.AddSingleton<IFileService, FileService>();
//}]}
            // Services
//{[{
            services.AddSingleton<IPersistAndRestoreService, PersistAndRestoreService>();
//}]}
        }
    }
}