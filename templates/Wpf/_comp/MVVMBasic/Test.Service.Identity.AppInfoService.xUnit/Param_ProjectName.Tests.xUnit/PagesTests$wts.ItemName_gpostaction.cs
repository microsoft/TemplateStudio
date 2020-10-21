//{[{
using Param_RootNamespace.Core.Contracts.Services;
//}]}
namespace Param_RootNamespace.Tests.xUnit
{
    public class PagesTests
    {
        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            // Core Services
//{[{
            services.AddSingleton<IIdentityService, IdentityService>();
            services.AddSingleton<IMicrosoftGraphService, MicrosoftGraphService>();
//}]}
            // Services
//{[{
            services.AddSingleton<IUserDataService, UserDataService>();
            services.AddSingleton<IIdentityCacheService, IdentityCacheService>();
            services.AddHttpClient("msgraph", client =>
            {
                client.BaseAddress = new System.Uri("https://graph.microsoft.com/v1.0/");
            });
//}]}
        }
    }
}