private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    // Services
//{[{
    services.AddSingleton<IApplicationInfoService, ApplicationInfoService>();
//}]}
}
