using Param_RootNamespace.Views;
using Param_RootNamespace.Services;

using System.Linq;
using System.Threading.Tasks;

using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Param_RootNamespace.Activation
{
    internal class CommandLineActivationHandler : ActivationHandler<CommandLineActivatedEventArgs>
    {
        // Learn more about these EventArgs at https://docs.microsoft.com/en-us/uwp/api/windows.applicationmodel.activation.commandlineactivatedeventargs
        protected override async Task HandleInternalAsync(CommandLineActivatedEventArgs args)
        {
            CommandLineActivationOperation operation = args.Operation;

            // Because these are supplied by the caller, they should be treated as untrustworthy.
            string cmdLineString = operation.Arguments;

            // The directory where the command-line activation request was made.
            // This is typically not the install location of the app itself, but could be any arbitrary path.
            string activationPath = operation.CurrentDirectoryPath;

            //// TODO WTS: parse the cmdLineString to determine what to do.
            //// If doing anything async, get a deferral first.
            //// using (var deferral = operation.GetDeferral())
            //// {
            ////     await ParseCmdString(cmdLineString, activationPath);
            //// }
            ////
            //// If the arguments warrant showing a different view on launch, that can be done here.
            //// NavigationService.Navigate(typeof(CmdLineActivationSamplePage), cmdLineString);
            //// If you do nothing, the app will launch like normal.

            await Task.CompletedTask;
        }

        protected override bool CanHandleInternal(CommandLineActivatedEventArgs args)
        {
            // Only handle a commandline launch if arguments are passed.
            return args?.Operation.Arguments.Any() ?? false;
        }
    }
}
