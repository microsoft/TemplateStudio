private System.IServiceProvider ConfigureServices()
{
    // Services
//{[{
    services.AddSingleton<ILocalSettingsService, LocalSettingsServiceUnpackaged>();
//}]}
}
