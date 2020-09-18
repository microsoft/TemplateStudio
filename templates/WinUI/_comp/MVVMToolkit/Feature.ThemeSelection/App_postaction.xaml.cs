private void ConfigureServices(IServiceCollection services)
{
    // Services
//{[{
    services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
//}]}
}
