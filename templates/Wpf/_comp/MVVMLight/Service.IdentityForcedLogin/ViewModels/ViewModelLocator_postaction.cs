//{[{
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
//}]}
namespace Param_RootNamespace.ViewModels
{
    public class ViewModelLocator
    {
//^^
//{[{
        public LogInViewModel LogInViewModel
            => SimpleIoc.Default.GetInstance<LogInViewModel>();
//}]}

        public ViewModelLocator()
        {
            // Core Services
//{[{
            SimpleIoc.Default.Register<IMicrosoftGraphService, MicrosoftGraphService>();
            SimpleIoc.Default.Register<IIdentityCacheService, IdentityCacheService>();
            SimpleIoc.Default.Register<IIdentityService, IdentityService>();
            SimpleIoc.Default.Register(() => GetHttpClientFactory());
//}]}
            // Services
//{[{
            SimpleIoc.Default.Register<IUserDataService, UserDataService>();
//}]}
            // Window
//{[{
            SimpleIoc.Default.Register<ILogInWindow, LogInWindow>();
            SimpleIoc.Default.Register<LogInViewModel>();
//}]}
        }
//^^
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
