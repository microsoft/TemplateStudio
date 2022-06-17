        .ConfigureServices((context, services) =>
        {
            // Services
//{[{
            services.AddSingleton<INotificationService, NotificationService>();
        }
//}]}
