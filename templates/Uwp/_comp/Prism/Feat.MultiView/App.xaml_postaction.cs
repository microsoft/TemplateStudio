//{**
// These code blocks add the WindowManagerService initialization to the App.xaml.cs of your project.
//**}
//{[{
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
//}]}

namespace Param_RootNamespace
{
    [Windows.UI.Xaml.Data.Bindable]
    public sealed partial class App : PrismUnityApplication
    {
        protected override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            await base.OnInitializeAsync(args);
//{[{
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                () =>
                {
                    WindowManagerService.Current.Initialize();
                });
//}]}
        }
    }
}
