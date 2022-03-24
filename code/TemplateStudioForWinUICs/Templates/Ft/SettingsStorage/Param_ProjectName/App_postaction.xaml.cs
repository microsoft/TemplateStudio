//{[{
using Param_RootNamespace.Models;
//}]}      

            .ConfigureServices((context, services) =>
            {
                // Configuration
//{[{
                services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
//}]}