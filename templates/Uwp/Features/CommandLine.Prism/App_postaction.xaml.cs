using Windows.UI.Xaml;
//^^
//{[{
using System.Linq;
//}]}

namespace Param_RootNamespace
{
    public sealed partial class App : PrismUnityApplication
    {
        protected override async Task OnActivateApplicationAsync(IActivatedEventArgs args)
        {
//^^
//{[{
            // Only handle a commandline launch if arguments are passed.
            if (args.Kind == ActivationKind.CommandLineLaunch && (((CommandLineActivatedEventArgs)args)?.Operation.Arguments.Any() ?? false) && args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                CommandLineActivatedEventArgs cmdLineArgs = (CommandLineActivatedEventArgs)args;
                CommandLineActivationOperation operation = cmdLineArgs.Operation;

                // Because these are supplied by the caller, they should be treated as untrustworthy.
                var cmdLineString = operation.Arguments;

                // The directory where the command-line activation request was made.
                // This is typically not the install location of the app itself, but could be any arbitrary path.
                var activationPath = operation.CurrentDirectoryPath;

                //// TODO WTS: parse the cmdLineString to determine what to do.
                //// If the arguments warrant showing a different view on launch, that can be done here.
                //// await LaunchApplicationAsync(PageTokens.OtherPage, cmdLineString);
                //// If you do nothing, the app will launch like normal.
            }
//}]}

            await Task.CompletedTask;
        }
    }
}
