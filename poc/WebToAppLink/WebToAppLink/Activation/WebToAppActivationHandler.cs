using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

using WebToAppLink.Services;
using WebToAppLink.Views;

namespace WebToAppLink.Activation
{
    internal class WebToAppActivationHandler : ActivationHandler<ProtocolActivatedEventArgs>
    {
        protected override async Task HandleInternalAsync(ProtocolActivatedEventArgs args)
        {
            switch (args.Uri.AbsolutePath)
            {
                case "/":
                    NavigationService.Navigate<MainPage>();
                    break;
                case "/windows/apps":
                    NavigationService.Navigate<WindowsAppsPage>();
                    break;
            }            
            await Task.CompletedTask;
        }

        protected override bool CanHandleInternal(ProtocolActivatedEventArgs args)
        {
            return args.Uri.Host == "developer.microsoft.com";
        }
    }
}
