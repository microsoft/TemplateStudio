//{[{
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Services;
//}]}
namespace Param_RootNamespace.Tests.xUnit
{
    public class PagesTests
    {
        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            // Services
//{[{
            services.AddSingleton<ISampleDataService, SampleDataService>();
//}]}
        }
    }
}