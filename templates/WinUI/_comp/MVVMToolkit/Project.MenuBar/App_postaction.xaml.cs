            .ConfigureServices((context, services) =>
            {
                // Services
//{[{
                services.AddSingleton<IRightPaneService, RightPaneService>();
//}]}

                // Views and ViewModels
//{[{
                services.AddTransient<ShellPage>();
                services.AddTransient<ShellViewModel>();
//}]}