private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
//^^
//{[{
    services.AddTransient<ts.ItemNameViewModel>();
    services.AddTransient<ts.ItemNamePage>();
//}]}
    // Configuration
}
