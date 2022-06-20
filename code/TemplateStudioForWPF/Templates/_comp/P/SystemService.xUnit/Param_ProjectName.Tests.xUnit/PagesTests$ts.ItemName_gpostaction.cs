//{[{
using Param_RootNamespace.Services;
using Param_RootNamespace.Contracts.Services;
//}]}
namespace Param_RootNamespace.Tests.XUnit;

public class PagesTests
{
    public PagesTests()
    {
        // App Services
//{[{
        _container.RegisterType<ISystemService, SystemService>();
//}]}
    }
}
