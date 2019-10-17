private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
//^^
//{[{
    services.AddSingleton<IWindowManagerService, WindowManagerService>();
    services.AddSingleton<IRightPaneService, RightPaneService>();
//}]}

    // Views and ViewModels
//^^
//{[{
    services.AddTransient<IShellDialogWindow, ShellDialogWindow>();
    services.AddTransient<ShellDialogViewModel>();
//}]}

    // Configuration
}
