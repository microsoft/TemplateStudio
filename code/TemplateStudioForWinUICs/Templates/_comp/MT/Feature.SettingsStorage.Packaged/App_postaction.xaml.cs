        ConfigureServices((context, services) =>
        {
            // Services
//{[{
            services.AddSingleton<ILocalSettingsService, LocalSettingsServicePackaged>();
//}]}
