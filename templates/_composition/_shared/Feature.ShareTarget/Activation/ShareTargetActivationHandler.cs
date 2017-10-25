using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Param_ItemNamespace.Views;

namespace Param_ItemNamespace.Activation
{
    internal class ShareTargetActivationHandler : ActivationHandler<ShareTargetActivatedEventArgs>
    {
        protected override async Task HandleInternalAsync(ShareTargetActivatedEventArgs args)
        {
            // Activation from ShareTarget opens the app as a new modal window which requires a new activation.
            var frame = new Frame();
            Window.Current.Content = frame;
            frame.Navigate(typeof(wts.ItemNameExamplePage), args.ShareOperation);
            Window.Current.Activate();

            await Task.CompletedTask;
        }
    }
}
