using Windows.UI.Xaml.Controls;

namespace Param_ItemNamespace.Views
{
    // TODO WTS: This page exists purely as an example of how to launch a specific page in response to a protocol launch and pass it a value. It is expected that you will delete this page once you have changed the handling of a protocol launch to meet your needs.
    public sealed partial class UriSchemeExamplePage : Page
    {
        public UriSchemeExamplePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

        // TODO: [ML] make the passed in page part of a composition template as will need different functionality for each framework
            // Do something with e.Parameter.ToString();
            
            /*
            var args = e.Parameter as ProtocolActivatedEventArgs;

            // Display the result of the protocol activation if we got here as a result of being activated for a protocol.
            if (args != null)
            {
                rootPage.NotifyUser("Protocol activation received. The received URI is " + args.Uri.AbsoluteUri + ".", NotifyType.StatusMessage);
            }
            */
        }
    }
}
