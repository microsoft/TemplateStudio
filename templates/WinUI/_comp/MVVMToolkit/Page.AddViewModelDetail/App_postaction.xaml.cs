private void ConfigureServices(IServiceCollection services)
{
//^^
//{[{
    services.AddTransient<wts.ItemNameDetailViewModel>();
    services.AddTransient<wts.ItemNameDetailPage>();
//}]}
}
