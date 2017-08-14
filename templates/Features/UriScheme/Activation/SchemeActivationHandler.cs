using Param_RootNamespace.Services;
using System.Threading.Tasks;

using Windows.ApplicationModel.Activation;

namespace Param_ItemNamespace
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
                // This will happen if the URI doens't contain a param called 'secret'
            }
 
            // It's also possible to have logic here to navigate to different pages. e.g. if you have logic based on the specific URI used to launch
            NavigationService.Navigate(typeof(Views.ExamplePage), secret);

            await Task.CompletedTask;
        }

        protected override bool CanHandleInternal(ProtocolActivatedEventArgs args)
        {
            // For this sample we only want to handle the URI if the path is set to the sample page.
            // If this check returns false the default handler will still be called and the app will be launched like normal.
            // TODO WTS: Update the logic here as appropraite to your app
            return args.Kind == ActivationKind.Protocol && args.Uri.Path.ToLowerInvariant().Equals("sample");
        }
    }
}
