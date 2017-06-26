using Param_RootNamespace.Services;
using System.Threading.Tasks;

using Windows.ApplicationModel.Activation;

namespace Param_ItemNamespace
{
    internal class SchemeActivationHandler : ActivationHandler<ProtocolActivatedEventArgs>
    {
        protected override async Task HandleInternalAsync(ProtocolActivatedEventArgs args)
        {
            // args.Uri.
            //  var file = args.Files.FirstOrDefault();


            /*
             * 
            FOR GITHUB

Having thought about how to implement this, I'm looking for feedback on plans to implement the following:

- The value for the protocol will be asked for in the wizard in the same way the name of a page can be specified. (New functionality for the validation rules and failure error message for this will be added to the template config)
- This feature will add another page (called ProtocolExample) to show passing information from a URI to a page within the app. This page is purely for demonstration and will include comments syaing that it is for sample purposes only and should be removed
- For NavPanel and Tabs&Pivot projects the activation handler will navigate to the example page within the shell/tab pages and have comments for how to navigate to a page not within the shell
- The example page will display parameters passed to it (if any)
- Comments in the top of the activation handler will list supported protocol content formats
- The activation handler will show a basic example of querying the arguments for valid content in the CanHandleInternal method
- The current activation document will be extended with more information about protocol launching

            */

            // TODO[ML]: add documentation about how to test protocol launching (Project Properties > Debug > do not launch but attach)
            // TODO[ML]: does this need any extra docs? or extend the activation docs?
            // TODO[ML]: also need to consider going to a page within the shell (or tab) - is this currently supported?
            // TODO[ML]: Add docs or examples for going to different pages or querying params
            // TODO[ML]: Need to consider if should include a special page accessing the passed params
            NavigationService.Navigate(typeof(Views.ExamplePage), "file");

            await Task.CompletedTask;
        }

        protected override bool CanHandleInternal(ProtocolActivatedEventArgs args)
        {
            // TODO[ML]: Add comment about also querying args to see if can handle this ok. (e.g. valid paths or required params)
            // TODO[ML]: Add comment that if this returns false the app will open normally
            return args.Kind == ActivationKind.Protocol;
        }
    }
}
