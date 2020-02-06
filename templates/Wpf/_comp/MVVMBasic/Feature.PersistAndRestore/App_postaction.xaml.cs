private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    // Services
//{[{
    services.AddSingleton<IPersistAndRestoreService, PersistAndRestoreService>();
//}]}
}
