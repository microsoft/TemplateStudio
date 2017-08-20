
namespace Param_ItemNamespace.Activation
{
    internal class SchemeActivationHandler : ActivationHandler<ProtocolActivatedEventArgs>
    {
//{[{
        // By default, this handler expects URIs of the format 'wtsapp:sample?secret={value}'
        protected override async Task HandleInternalAsync(ProtocolActivatedEventArgs args)
        {
            if (args.Uri.AbsolutePath.ToLowerInvariant().Equals("sample"))
            {
                var secret = "<<I-HAVE-NO-SECRETS>>";

                try
                {
                    if (args.Uri.Query != null)
                    {
                        // The following will extract the secret value and pass it to the page. Alternatively, you could pass all or some of the Uri.
                        var decoder = new Windows.Foundation.WwwFormUrlDecoder(args.Uri.Query);

                        secret = decoder.GetFirstValueByName("secret");
                    }
                }
                catch (ArgumentException)
                {
                    // This will happen if the URI doens't contain a param called 'secret'
                }

                // It's also possible to have logic here to navigate to different pages. e.g. if you have logic based on the URI used to launch
                NavigationService.Navigate(typeof(Views.wts.ItemNamePage), secret);
            }
            else
            {
                // If not navigating to a specific page based on the URI, navigate to the home page
                NavigationService.Navigate(typeof(Views.Param_HomeNamePage));
            }

            await Task.CompletedTask;
        }

//}]}
    }
}
