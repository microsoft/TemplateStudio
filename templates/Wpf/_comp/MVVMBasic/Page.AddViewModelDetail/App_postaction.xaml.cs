private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
//^^
//{[{
    services.AddTransient<wts.ItemNameDetailViewModel>();
    services.AddTransient<wts.ItemNameDetailPage>();
//}]}
    // Configuration
}
