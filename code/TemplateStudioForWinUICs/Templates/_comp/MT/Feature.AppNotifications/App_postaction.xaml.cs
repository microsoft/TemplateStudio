//{[{
using Param_RootNamespace.Notifications;
//}]}
        .CreateDefaultBuilder()
//{[{
        .UseContentRoot(AppContext.BaseDirectory)
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
        App.GetService<INotificationService>()!.Show(string.Format("AppNotificationSamplePayload".GetLocalized(), AppContext.BaseDirectory));
//}]}
