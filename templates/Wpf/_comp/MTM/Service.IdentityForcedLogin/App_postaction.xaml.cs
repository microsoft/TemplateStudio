private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    // App Host
    services.AddHostedService<ApplicationHostService>();
//{[{
    services.AddSingleton<IIdentityCacheService, IdentityCacheService>();
    services.AddHttpClient("msgraph", client =>
    {
        client.BaseAddress = new System.Uri("https://graph.microsoft.com/v1.0/");
    });
//}]}
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
