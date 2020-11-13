//{[{
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Services;
//}]}
namespace Param_RootNamespace.Tests.NUnit
{
    public class PagesTests
    {
        [SetUp]
        public void Setup()
        {
            // Core Services
//{[{
            SimpleIoc.Default.Register<ISampleDataService, SampleDataService>();
//}]}
        }
    }
}