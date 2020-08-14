using System;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Toolkit.Uwp.Notifications;

namespace Param_RootNamespace.Activation
{
    // The GUID CLSID must be unique to your app. Create a new GUID if copying this code.
    [ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces(typeof(INotificationActivationCallback))]
    [Guid("8770A9EE-9EF3-4D0E-B85C-1CD0994763D3")]
    [ComVisible(true)]
    public class ToastNotificationActivator : NotificationActivator
    {
        public override async void OnActivated(string arguments, NotificationUserInput userInput, string appUserModelId)
        {
            await Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                var app = Application.Current as App;
                var config = app.GetService<IConfiguration>();
                config[ToastNotificationActivationHandler.ActivationArguments] = arguments;
                await app.StartAsync();
            });
        }
    }
}
