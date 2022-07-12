//{[{
using Param_RootNamespace.Notifications;
//}]}
        ConfigureServices((context, services) =>
        {
            // Other Activation Handlers
//{[{
            services.AddTransient<IActivationHandler, AppNotificationActivationHandler>();
//}]}
            // Services
//{[{
            services.AddSingleton<IAppNotificationService, AppNotificationService>();
//}]}
        Build();
//{[{

        App.GetService<IAppNotificationService>().Initialize();
//}]}
        base.OnLaunched(args);
//{[{

        App.GetService<IAppNotificationService>().Show(string.Format("AppNotificationSamplePayload".GetLocalized(), AppContext.BaseDirectory));
//}]}
