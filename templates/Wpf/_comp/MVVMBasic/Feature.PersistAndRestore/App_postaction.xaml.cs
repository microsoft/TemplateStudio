private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
//^^
//{[{
    services.AddSingleton<IPersistAndRestoreService, PersistAndRestoreService>();
//}]}

    // Views and ViewModels
}
