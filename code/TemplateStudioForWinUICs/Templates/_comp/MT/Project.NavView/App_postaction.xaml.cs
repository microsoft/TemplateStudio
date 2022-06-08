        .ConfigureServices((context, services) =>
        {
            // Services
//{[{
            services.AddTransient<INavigationViewService, NavigationViewService>();
//}]}

            // Views and ViewModels
//{[{
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();
//}]}
