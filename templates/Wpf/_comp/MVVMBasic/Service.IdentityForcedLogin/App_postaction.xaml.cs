private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    // Core Services
    services.AddSingleton<IFilesService, FilesService>();
//{[{
    services.AddSingleton<IMicrosoftGraphService, MicrosoftGraphService>();
    services.AddSingleton<IIdentityService, IdentityService>();
//}]}
//^^
//{[{
    services.AddTransient<ILogInWindow, LogInWindow>();
    services.AddTransient<LogInViewModel>();
//}]}
    // Configuration
}
