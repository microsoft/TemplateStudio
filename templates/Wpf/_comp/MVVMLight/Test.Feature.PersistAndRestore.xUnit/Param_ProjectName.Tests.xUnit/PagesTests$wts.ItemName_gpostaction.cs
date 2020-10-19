//{[{
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Services;
//}]}
namespace Param_RootNamespace.Tests.xUnit
{
    public class PagesTests
    {
        public PagesTests()
        {
            // Core Services
//{[{
            SimpleIoc.Default.Register<IFileService, FileService>();
//}]}
            // Services
//{[{
            SimpleIoc.Default.Register<IPersistAndRestoreService, PersistAndRestoreService>();
//}]}
        }
    }
}