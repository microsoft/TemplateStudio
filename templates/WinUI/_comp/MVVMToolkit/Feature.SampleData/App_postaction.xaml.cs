//{[{
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Services;
//}]}
namespace Param_RootNamespace
{
    public partial class App : Application
    {
        private void ConfigureServices(IServiceCollection services)
        {
            // Core Services
//{[{
            services.AddSingleton<ISampleDataService, SampleDataService>();
//}]}
        }
    }
}
