//{[{
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Param_RootNamespace.Core.Contracts.Services;
//}]}
namespace Param_RootNamespace.Tests.NUnit
{
    public class PagesTests
    {
        [SetUp]
        public void Setup()
        {
            // Core Services
//{[{
            SimpleIoc.Default.Register<IIdentityService, IdentityService>();
            SimpleIoc.Default.Register<IMicrosoftGraphService, MicrosoftGraphService>();
//}]}
            // Services
//{[{
            SimpleIoc.Default.Register<IUserDataService, UserDataService>();
            SimpleIoc.Default.Register<IIdentityCacheService, IdentityCacheService>();
            SimpleIoc.Default.Register(() => GetHttpClientFactory());
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