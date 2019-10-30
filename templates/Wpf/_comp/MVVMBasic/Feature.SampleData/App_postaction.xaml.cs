private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
//^^
//{[{
    services.AddSingleton<ISampleDataService, SampleDataService>();
//}]}

    // Views and ViewModels
}
