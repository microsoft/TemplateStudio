private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
//^^
//{[{
    services.AddTransient<wts.ItemNameViewModel>();
    services.AddTransient<wts.ItemNamePage>();
//}]}
    // Configuration
}
