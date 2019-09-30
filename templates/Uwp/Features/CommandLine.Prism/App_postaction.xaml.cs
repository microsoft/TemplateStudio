using Windows.UI.Xaml;
//^^
//{[{
using System.Linq;
//}]}

namespace Param_RootNamespace
{
    public sealed partial class App : PrismUnityApplication
    {
        public App()
        {
            InitializeComponent();
        }

//{[{
        // Used to store details between when UWP makes them available and when Prism convention wants to use them.
        private (string activationPath, string arguments) cmdLineDetails;

        protected override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);

            // Details of the command line launch can only be accessed during App.OnActivated.
            // Storing the details for use when needed in `OnActivateApplicationAsync`.
            if (args.Kind == ActivationKind.CommandLineLaunch)
            {
                var operation = ((CommandLineActivatedEventArgs)args).Operation;
                cmdLineDetails = (operation.CurrentDirectoryPath, operation.Arguments);
            }
        }
//}]}
        protected override async Task OnActivateApplicationAsync(IActivatedEventArgs args)
        {
//^^
//{[{
            // Only handle a commandline launch if arguments are passed.
            // Learn more about these EventArgs at https://docs.microsoft.com/en-us/uwp/api/windows.applicationmodel.activation.commandlineactivatedeventargs
            if (args.Kind == ActivationKind.CommandLineLaunch && !string.IsNullOrWhiteSpace(cmdLineDetails.arguments) && args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                // Because these are supplied by the caller, they should be treated as untrustworthy.
                var cmdLineString = cmdLineDetails.arguments;

                // The directory where the command-line activation request was made.
                // This is typically not the install location of the app itself, but could be any arbitrary path.
                var activationPath = cmdLineDetails.activationPath;

                //// TODO WTS: parse the cmdLineString to determine what to do.
                //// If the arguments warrant showing a different view on launch, that can be done here.
                //// await LaunchApplicationAsync(PageTokens.CmdLineActivationSamplePage, cmdLineString);
                //// If you do nothing, the app will launch like normal.
            }
//}]}

            await Task.CompletedTask;
        }
    }
}
