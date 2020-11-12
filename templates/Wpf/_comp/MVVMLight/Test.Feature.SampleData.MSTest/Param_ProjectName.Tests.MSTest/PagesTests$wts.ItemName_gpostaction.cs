//{[{
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Services;
//}]}
namespace Param_RootNamespace.Tests.MSTest
{
    [TestClass]
    public class PagesTests
    {
        public PagesTests()
        {
            // Core Services
//{[{
            SimpleIoc.Default.Register<ISampleDataService, SampleDataService>();
//}]}
        }
    }
}