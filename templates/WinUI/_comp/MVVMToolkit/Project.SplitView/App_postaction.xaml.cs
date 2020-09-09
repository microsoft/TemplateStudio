//{[{
using Param_RootNamespace.Contracts.Views;
//}]}

namespace Param_RootNamespace
{
    public partial class App : Application
    {
        private void ConfigureServices(IServiceCollection services)
        {
//{[{
            // Views and ViewModels
            services.AddTransient<IShellWindow, ShellWindow>();
            services.AddTransient<ShellViewModel>();
//}]}
        }
    }
}

