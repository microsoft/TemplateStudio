private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    // Services
//{[{
    services.AddSingleton<IWindowManagerService, WindowManagerService>();
//}]}
    // Views
//^^
//{[{
    services.AddTransient<IShellDialogWindow, ShellDialogWindow>();
//}]}
    // Configuration
}
