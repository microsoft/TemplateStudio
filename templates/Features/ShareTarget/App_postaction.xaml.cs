//{[{
using Windows.UI.Xaml.Controls;
//}]}
//{**
// This code block adds the OnShareTargetActivated event handler to the App class of your project.
//**}
namespace Param_RootNamespace
{
    public sealed partial class App : Application
    {
//^^
//{[{

        protected override void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
        {
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
