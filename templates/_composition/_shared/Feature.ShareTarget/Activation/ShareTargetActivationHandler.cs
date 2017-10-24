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
            var frame = new Frame();
            Window.Current.Content = frame;
            frame.Navigate(typeof(wts.ItemNameExamplePage), args.ShareOperation);

            await Task.CompletedTask;
        }
    }
}
