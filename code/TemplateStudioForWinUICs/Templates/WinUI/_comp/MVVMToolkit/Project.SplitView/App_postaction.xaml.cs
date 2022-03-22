private System.IServiceProvider ConfigureServices()
{
    // Services
//{[{
    services.AddTransient<INavigationViewService, NavigationViewService>();
//}]}

    // Views and ViewModels
//{[{
    services.AddTransient<ShellPage>();
    services.AddTransient<ShellViewModel>();
//}]}
}
