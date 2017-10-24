namespace Param_ItemNamespace.Services
{
    internal class ActivationService
    {
        //^^
        //{[{
        public void ActivateFromShareTarget(Type sourcePageType, ShareTargetActivatedEventArgs args)
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

            rootFrame.Navigate(sourcePageType, args.ShareOperation);
            Window.Current.Activate();
        }

        //}]}
        private async Task InitializeAsync()
        {
        }
    }
}
