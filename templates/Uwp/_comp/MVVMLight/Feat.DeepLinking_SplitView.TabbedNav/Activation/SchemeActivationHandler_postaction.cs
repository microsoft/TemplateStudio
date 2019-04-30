//{[{
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
                NavigationService.Navigate(data.ViewModelName, data.Parameters);
            }

            await Task.CompletedTask;
        }

//}]}
    }
}
