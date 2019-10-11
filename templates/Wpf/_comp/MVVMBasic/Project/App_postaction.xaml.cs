private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
//^^
//{[{
    services.AddSingleton<INavigationService, NavigationService>();
//}]}
    // Views and ViewModels
}
