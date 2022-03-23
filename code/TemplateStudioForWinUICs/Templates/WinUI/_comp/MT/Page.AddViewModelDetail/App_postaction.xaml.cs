private System.IServiceProvider ConfigureServices()
{
//^^
//{[{
    services.AddTransient<wts.ItemNameDetailViewModel>();
    services.AddTransient<wts.ItemNameDetailPage>();
//}]}
    return services.BuildServiceProvider();
}
