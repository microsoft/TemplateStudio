//{[{
using Caliburn.Micro;
using Param_RootNamespace.ViewModels;
//}]}
namespace Param_RootNamespace.Activation
{
    internal class SchemeActivationHandler : ActivationHandler<ProtocolActivatedEventArgs>
    {
//{[{
        private INavigationService _navigationService;

        public SchemeActivationHandler(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        // By default, this handler expects URIs of the format 'wtsapp:sample?paramName1=paramValue1&paramName2=paramValue2'
        protected override async Task HandleInternalAsync(ProtocolActivatedEventArgs args)
        {
            // Create data from activation Uri in ProtocolActivatedEventArgs
            var data = new SchemeActivationData(args.Uri);
            if (data.IsValid)
            {
                _navigationService.Navigate(data.PageType, data.Parameters);
            }
            else if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                // If the app isn't running and not navigating to a specific page based on the URI, navigate to the home page
                _navigationService.For<Param_HomeNameViewModel>().Navigate();
            }

            await Task.CompletedTask;
        }

//}]}
    }
}
