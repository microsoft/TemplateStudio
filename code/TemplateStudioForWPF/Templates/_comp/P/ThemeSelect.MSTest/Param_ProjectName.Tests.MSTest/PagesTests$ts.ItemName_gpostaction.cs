//{[{
using Param_RootNamespace.Services;
using Param_RootNamespace.Contracts.Services;
//}]}
namespace Param_RootNamespace.Tests.MSTest;

[TestClass]
public class PagesTests
{
    public PagesTests()
    {
        // App Services
//{[{
        _container.RegisterType<IThemeSelectorService, ThemeSelectorService>();
//}]}
    }
}
