private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
//^^
//{[{
    services.AddSingleton<IMicrosoftGraphService, MicrosoftGraphService>();
    services.AddSingleton<IIdentityService, IdentityService>();
//}]}
    // Services
//^^
//{[{
    services.AddSingleton<IUserDataService, UserDataService>();
//}]}
    // Views and ViewModels
//^^
//{[{
    services.AddTransient<ILogInWindow, LogInWindow>();
    services.AddTransient<LogInViewModel>();
//}]}
    // Configuration
}
