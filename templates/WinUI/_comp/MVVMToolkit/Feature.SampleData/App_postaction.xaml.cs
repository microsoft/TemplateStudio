//{[{
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Services;
//}]}
namespace Param_RootNamespace
{
    public partial class App : Application
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
