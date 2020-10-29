//{[{
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Param_RootNamespace.Core.Contracts.Services;
//}]}
namespace Param_RootNamespace.Tests.XUnit
{
    public class PagesTests
    {
        public PagesTests()
        {
            // Core Services
//{[{
            _container.RegisterType<IIdentityService, IdentityService>();
            _container.RegisterType<IMicrosoftGraphService, MicrosoftGraphService>();
//}]}
            // App Services
//{[{
            _container.RegisterType<IUserDataService, UserDataService>();
            _container.RegisterType<IIdentityCacheService, IdentityCacheService>();
            _container.RegisterFactory<IHttpClientFactory>(container => GetHttpClientFactory());
//}]}
        }
//{[{
        private IHttpClientFactory GetHttpClientFactory()
        {
            var services = new ServiceCollection();
            services.AddHttpClient("msgraph", client =>
            {
                client.BaseAddress = new System.Uri("https://graph.microsoft.com/v1.0/");
            });

            return services.BuildServiceProvider().GetRequiredService<IHttpClientFactory>();
        }
//}]}
    }
}