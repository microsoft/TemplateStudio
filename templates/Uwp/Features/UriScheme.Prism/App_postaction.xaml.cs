using Windows.UI.Xaml;
//^^
//{[{
using Windows.UI.Xaml.Controls;
using Prism.Windows.Navigation;
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
            if (args.Kind == ActivationKind.Protocol && ((ProtocolActivatedEventArgs)args)?.Uri == null && args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                var protocolArgs = args as ProtocolActivatedEventArgs;
                if (protocolArgs.Uri.AbsolutePath.Equals("sample", StringComparison.OrdinalIgnoreCase))
                {
                    var secret = "<<I-HAVE-NO-SECRETS>>";

                    try
                    {
                        if (protocolArgs.Uri.Query != null)
                        {
                            // The following will extract the secret value and pass it to the page. Alternatively, you could pass all or some of the Uri.
                            var decoder = new Windows.Foundation.WwwFormUrlDecoder(protocolArgs.Uri.Query);

                            secret = decoder.GetFirstValueByName("secret");
                        }
                    }
                    catch (Exception)
                    {
                        // NullReferenceException if the URI doesn't contain a query
                        // ArgumentException if the query doesn't contain a param called 'secret'
                    }

                    // It's also possible to have logic here to navigate to different pages. e.g. if you have logic based on the URI used to launch
                    await LaunchApplicationAsync(PageTokens.UriSchemeExamplePage, secret);
                }
                else
                {
                    // If the app isn't running and not navigating to a specific page based on the URI, navigate to the home page
                    await OnLaunchApplicationAsync(args as LaunchActivatedEventArgs);
                }
            }
//}]}

            await Task.CompletedTask;
        }
    }
}
