using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace Param_RootNamespace.Activation
{
    internal class WebToAppLinkActivationHandler : ActivationHandler<ProtocolActivatedEventArgs>
    {
        // See detailed documentation and samples about Web to App link
        // https://docs.microsoft.com/windows/uwp/launch-resume/web-to-app-linking
        // https://blogs.windows.com/buildingapps/2016/10/14/web-to-app-linking-with-appurihandlers/
        //
        // TODO WTS: Update the Host URI here and in Package.appxmanifest XML (Right click > View Code)
        private const string Host = "myapp.website.com";
        private const string Section1 = "/MySection1";
        private const string Section2 = "/MySection2";

        protected override async Task HandleInternalAsync(ProtocolActivatedEventArgs args)
        {
            // TODO WTS: Handle navigation based on the original URI
            // Use args.Uri.AbsolutePath to determinate the page you want to launch the application.
            switch (args.Uri.AbsolutePath)
            {
                // Open the page in app that is equivalent to the section on the website.
                case Section1:
                    // Use NavigationService to Navigate to MySection1Page
                    break;
                case Section2:
                    // Use NavigationService to Navigate to MySection2Page
                    break;
                default:
                    // Launch the application with default page.
                    // Use NavigationService to Navigate to MainPage
                    break;
            }

            await Task.CompletedTask;
        }

        protected override bool CanHandleInternal(ProtocolActivatedEventArgs args)
        {
            return args?.Uri?.Host == Host;
        }
    }
}
