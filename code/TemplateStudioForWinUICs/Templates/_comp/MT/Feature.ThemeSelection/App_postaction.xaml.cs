            .ConfigureServices((context, services) =>
            {
                // Services
//{[{
                services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
//}]}