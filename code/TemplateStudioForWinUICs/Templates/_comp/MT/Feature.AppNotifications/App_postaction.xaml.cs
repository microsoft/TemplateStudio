//{[{
using Param_RootNamespace.Notifications;
//}]}
        .CreateDefaultBuilder()
//{[{
        .UseContentRoot(System.AppContext.BaseDirectory)
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
        App.GetService<INotificationService>()!.Initialize();
        App.GetService<INotificationService>()!.Show("AppNotificationSamplePayload".GetLocalized());
//}]}
