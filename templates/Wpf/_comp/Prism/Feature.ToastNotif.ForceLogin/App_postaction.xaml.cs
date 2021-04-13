namespace Param_RootNamespace
{
    public partial class App : PrismApplication
    {
        protected override async void OnInitialized()
        {
            // https://docs.microsoft.com/windows/uwp/design/shell/tiles-and-notifications/send-local-toast?tabs=desktop
            ToastNotificationManagerCompat.OnActivated += (toastArgs) =>
            {
                    config[App.ToastNotificationActivationArguments] = toastArgs.Argument;
//{[{
                    var userLogged = await TryUserLogin();

                    if (!userLogged)
                    {
                        return;
                    }
//}]}
                    App.Current.MainWindow.Show();
                    App.Current.MainWindow.Activate();
                    if (App.Current.MainWindow.WindowState == WindowState.Minimized)
                    {
                        App.Current.MainWindow.WindowState = WindowState.Normal;
                    }

                    await Task.CompletedTask;
                });
            };
        }
    }
}
