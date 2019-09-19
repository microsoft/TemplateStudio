//{**
// These code blocks add the WindowManagerService initialization to the App.xaml.cs of your project.
//**}
protected override async Task OnInitializeAsync(IActivatedEventArgs args)
{
//^^
//{[{
    await WindowManagerService.Current.InitializeAsync();
//}]}
}
