//{[{
using Windows.UI.Xaml.Controls;
//}]}
//{**
// This code block adds the OnShareTargetActivated event handler to the App class of your project.
//**}
namespace Param_RootNamespace
{
    public sealed partial class App
    {
//^^
//{[{

        protected override void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
        {
            // See more about configure share target in UWP
            // https://docs.microsoft.com/en-us/windows/uwp/app-to-app/receive-data
            // See also how to share data from you App
            // https://docs.microsoft.com/en-us/windows/uwp/app-to-app/share-data
            var rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                Window.Current.Content = rootFrame;
            }

            rootFrame.Navigate(typeof(Views.ShareTargetFeatureExamplePage), args.ShareOperation);
            Window.Current.Activate();
        }
//}]}
    }
}
