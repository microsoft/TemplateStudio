//{[{
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Services;
//}]}
namespace Param_RootNamespace
{
    {
        private System.IServiceProvider ConfigureServices()
        {
            // Core Services
//{[{
            services.AddSingleton<ISampleDataService, SampleDataService>();
//}]}
        }
    }
}
