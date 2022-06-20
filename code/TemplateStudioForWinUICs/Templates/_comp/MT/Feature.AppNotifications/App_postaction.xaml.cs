//{[{
using Param_RootNamespace.Notifications;
//}]}
        .ConfigureServices((context, services) =>
        {
            // Other Activation Handlers
//{[{
            services.AddTransient<IActivationHandler, NotificationActivationHandler>();
//}]}
            // Services
//{[{
            services.AddSingleton<INotificationService, NotificationService>();
//}]}
        base.OnLaunched(args);
//{[{
        App.GetService<INotificationService>().Initialize();
//}]}
