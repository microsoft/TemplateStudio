private void ConfigureServices(IServiceCollection services)
{
//^^
//{[{
    services.AddTransient<wts.ItemNameViewModel>();
    services.AddTransient<wts.ItemNamePage>();
//}]}
}
