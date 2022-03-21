private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
//^^
//{[{
    services.AddSingleton<IRightPaneService, RightPaneService>();
//}]}
}
