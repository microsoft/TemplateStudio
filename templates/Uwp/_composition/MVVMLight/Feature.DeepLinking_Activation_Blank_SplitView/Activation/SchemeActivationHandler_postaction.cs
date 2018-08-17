//{[{
using Param_ItemNamespace.Views;
//}]}
namespace Param_ItemNamespace.Activation
{
    internal class SchemeActivationHandler : ActivationHandler<ProtocolActivatedEventArgs>
    {
//{[{
        private NavigationServiceEx NavigationService
        {
            get
            {
                return CommonServiceLocator.ServiceLocator.Current.GetInstance<NavigationServiceEx>();
            }
        }

        // By default, this handler expects URIs of the format 'wtsapp:sample?secret={value}'
        protected override async Task HandleInternalAsync(ProtocolActivatedEventArgs args)
        {
            // Create data from activation Uri in ProtocolActivatedEventArgs
            var data = new SchemeActivationData(args.Uri);
            if (data.IsValid)
            {
                NavigationService.Navigate(data.ViewModelName, data.Parameters);
            }
            else if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                // If the app isn't running and not navigating to a specific page based on the URI, navigate to the home page
                NavigationService.Navigate(typeof(ViewModels.Param_HomeNameViewModel).FullName);
            }

            await Task.CompletedTask;
        }

//}]}
    }
}
