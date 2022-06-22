//{[{
using Param_RootNamespace.Services;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Services;
//}]}
namespace Param_RootNamespace.Tests.NUnit;

public class PagesTests
{
    [SetUp]
    public void Setup()
    {
        // App Services
//{[{
        _container.RegisterType<ISampleDataService, SampleDataService>();
//}]}
    }
}
