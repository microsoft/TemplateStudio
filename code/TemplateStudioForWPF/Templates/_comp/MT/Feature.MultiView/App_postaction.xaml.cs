private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    // Services
//{[{
    services.AddSingleton<IWindowManagerService, WindowManagerService>();
//}]}
    // Views and ViewModels
//^^
//{[{
    services.AddTransient<IShellDialogWindow, ShellDialogWindow>();
    services.AddTransient<ShellDialogViewModel>();
//}]}
    // Configuration
}
