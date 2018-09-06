//{**
// These code blocks add the WindowManagerService initialization to the App.xaml.cs of your project.
//**}

protected override async Task OnInitializeAsync(IActivatedEventArgs args)
{
    //{[{
    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
    () =>
    {
        WindowManagerService.Current.Initialize();
    });
    //}]}
}
