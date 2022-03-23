private System.IServiceProvider ConfigureServices()
{
//^^
//{[{
    services.AddTransient<wts.ItemNameViewModel>();
    services.AddTransient<wts.ItemNamePage>();
//}]}
    return services.BuildServiceProvider();
}
