//{[{
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Services;
//}]}

        ConfigureServices((context, services) =>
        {
            // Core Services
//{[{
            services.AddSingleton<ISampleDataService, SampleDataService>();
//}]}
