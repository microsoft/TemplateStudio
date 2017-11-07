using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace WebToAppLink.Activation
{
    internal class WebToAppActivationHandler : ActivationHandler<ProtocolActivatedEventArgs>
    {
        // https://docs.microsoft.com/en-us/windows/uwp/launch-resume/web-to-app-linking
        // TODO WTS: Set the same Host Uri on Package.appxmanifest
        private const string Host = "myapp.website.com";
        private const string Section1 = "/MySection1";
        private const string Section2 = "/MySection2";

        protected override async Task HandleInternalAsync(ProtocolActivatedEventArgs args)
        {
            // TODO WTS: Use args.Uri.AbsolutePath to determinate the page you want to launch the application.            
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
