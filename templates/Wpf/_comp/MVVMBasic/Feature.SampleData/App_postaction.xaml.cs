//{[{
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Services;
//}]}
namespace Param_RootNamespace
{
    public partial class App : Application
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
