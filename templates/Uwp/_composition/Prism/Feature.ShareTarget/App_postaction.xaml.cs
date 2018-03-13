//{**
// This code block adds the OnShareTargetActivated event handler to the App class of your project.
//**}
//^^
//{[{
using Windows.UI.Xaml.Controls;
using Param_RootNamespace.Views;
//}]}
namespace Param_RootNamespace
{
    public sealed partial class App : PrismUnityApplication
    {
//^^
//{[{

        protected override async void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
        {
            // Activation from ShareTarget opens the app as a new modal window which requires a new activation.
            var frame = new Frame();
            Window.Current.Content = frame;
            frame.Navigate(typeof(wts.ItemNamePage), args.ShareOperation);
            Window.Current.Activate();

            await Task.CompletedTask;
        }
//}]}
    }
}
