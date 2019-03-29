//{[{
using Param_RootNamespace.Helpers;
using Param_RootNamespace.Views;
using Param_RootNamespace.ViewModels;
//}]}
namespace Param_RootNamespace.Activation
{
    internal class SchemeActivationHandler : ActivationHandler<ProtocolActivatedEventArgs>
    {
//{[{
        public NavigationServiceEx NavigationService => ViewModelLocator.Current.NavigationService;

        // By default, this handler expects URIs of the format 'wtsapp:sample?paramName1=paramValue1&paramName2=paramValue2'
        protected override async Task HandleInternalAsync(ProtocolActivatedEventArgs args)
        {
            // Create data from activation Uri in ProtocolActivatedEventArgs
            var data = new SchemeActivationData(args.Uri);
            if (data.IsValid)
            {
                MenuNavigationHelper.UpdateView(data.ViewModelName, data.Parameters);
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
