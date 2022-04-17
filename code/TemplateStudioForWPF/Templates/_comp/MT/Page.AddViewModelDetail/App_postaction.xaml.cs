private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    //^^
    //{[{
    services.AddTransient<ts.ItemNameDetailViewModel>();
    services.AddTransient<ts.ItemNameDetailPage>();
    //}]}
    // Configuration
}
