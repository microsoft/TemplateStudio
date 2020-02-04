private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    // Core Services
//{[{
    services.AddSingleton<IMicrosoftGraphService, MicrosoftGraphService>();
    services.AddSingleton<IIdentityService, IdentityService>();
//}]}
    // Services
//{[{
    services.AddSingleton<IUserDataService, UserDataService>();
//}]}
//^^
//{[{
    services.AddTransient<ILogInWindow, LogInWindow>();
    services.AddTransient<LogInViewModel>();
//}]}
    // Configuration
}
