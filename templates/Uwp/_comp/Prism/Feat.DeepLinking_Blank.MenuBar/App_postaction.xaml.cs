using Windows.UI.Xaml;
//^^
//{[{
using Windows.UI.Xaml.Controls;
using Prism.Windows.Navigation;
using Param_RootNamespace.Activation;
using Param_RootNamespace.Views;
//}]}

namespace Param_RootNamespace
{
    public sealed partial class App : PrismUnityApplication
    {
        public App()
        {
            InitializeComponent();
        }

        protected override async Task OnActivateApplicationAsync(IActivatedEventArgs args)
        {
//{[{
            // By default, this handler expects URIs of the format 'wtsapp:sample?paramName1=paramValue1&paramName2=paramValue2'
            if (args.Kind == ActivationKind.Protocol && args is ProtocolActivatedEventArgs protocolArgs && protocolArgs.Uri != null)
            {
                // Create data from activation Uri in ProtocolActivatedEventArgs
                var data = new SchemeActivationData(protocolArgs.Uri);
                if (data.IsValid)
                {
                    await LaunchApplicationAsync(data.PageToken, data.Parameters);
                }
                else if (args.PreviousExecutionState != ApplicationExecutionState.Running)
                {
                    // If the app isn't running and not navigating to a specific page based on the URI, navigate to the home page
                    await OnLaunchApplicationAsync(args as LaunchActivatedEventArgs);
                }
            }
//}]}
//{--{
            await Task.CompletedTask;
//}--}
        }
    }
}
