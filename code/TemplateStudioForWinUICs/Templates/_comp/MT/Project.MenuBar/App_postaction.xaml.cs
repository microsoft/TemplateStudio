        .ConfigureServices((context, services) =>
        {
            // Views and ViewModels
//{[{
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();
//}]}
