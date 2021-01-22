namespace Param_RootNamespace
{
    public partial class App : Application
    {
        private System.IServiceProvider ConfigureServices()
        {
            // Services
//{[{
            services.AddTransient<IWebViewService, WebViewService>();
//}]}
        }
    }
}
