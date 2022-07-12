//{[{
using Param_RootNamespace.Notifications;
//}]}
        ConfigureServices((context, services) =>
        {
            // Other Activation Handlers
//{[{
            services.AddTransient<IActivationHandler, NotificationActivationHandler>();
//}]}
            // Services
//{[{
            services.AddSingleton<INotificationService, NotificationService>();
//}]}
        Build();
//{[{

        App.GetService<INotificationService>().Initialize();
//}]}
        base.OnLaunched(args);
//{[{

        App.GetService<INotificationService>().Show(string.Format("AppNotificationSamplePayload".GetLocalized(), AppContext.BaseDirectory));
//}]}
