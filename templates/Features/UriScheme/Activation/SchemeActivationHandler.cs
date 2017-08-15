using System;
using System.Threading.Tasks;
using Param_RootNamespace.Services;
using Windows.ApplicationModel.Activation;

namespace Param_ItemNamespace.Activation
{
    // TODO WTS: Open package.appxmanifest and change the declaration for the scheme (from the default of 'wtsapp') to what you want for your app.
    // More details about this functionality can be found at https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/features/uri-scheme.md
    // TODO WTS: Change the image in Assets/Logo.png to one for display if the OS asks the user which app to launch.
    internal class SchemeActivationHandler : ActivationHandler<ProtocolActivatedEventArgs>
    {
        // This handler expects URIs of the format 'wtsapp:sample?secret={value}'
        protected override async Task HandleInternalAsync(ProtocolActivatedEventArgs args)
        {
            // The following will extract the secret value and pass it to the page. Alternatively, you could pass all or some of the Uri.
            var decoder = new Windows.Foundation.WwwFormUrlDecoder(args.Uri.Query);

            var secret = "<<I-HAVE-NO-SECRETS>>";

            try 
            {
                decoder.GetFirstValueByName("secret");
            }
            catch (ArgumentException)
            {
                // This will happen if the URI Query doesn't contain a param called 'secret'
            }
 
            // It's also possible to have logic here to navigate to different pages. e.g. if you have logic based on the specific URI used to launch
            NavigationService.Navigate(typeof(Views.UriSchemeExamplePage), secret);

            await Task.CompletedTask;
        }

        protected override bool CanHandleInternal(ProtocolActivatedEventArgs args)
        {
            // If your app has multiple handlers of ProtocolActivationEventArgs
            // use this method to determine which to use. (possibly checking args.Uri.Scheme)
            return true;
        }
    }
}
