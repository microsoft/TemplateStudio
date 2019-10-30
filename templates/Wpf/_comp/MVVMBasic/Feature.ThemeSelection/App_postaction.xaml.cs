private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
//^^
//{[{
    services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
//}]}

    // Views and ViewModels
}
