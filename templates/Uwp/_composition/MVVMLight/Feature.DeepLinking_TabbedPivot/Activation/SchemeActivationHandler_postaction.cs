//{[{
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Param_ItemNamespace.Views;
using Param_ItemNamespace.ViewModels;
//}]}
namespace Param_ItemNamespace.Activation
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
                var frame = Window.Current.Content as Frame;
                if (frame.Content is PivotPage pivotPage)
                {
                    await pivotPage.InitializeFromSchemeActivationAsync(data);
                }
                else
                {
                    NavigationService.Navigate(typeof(ViewModels.PivotViewModel).FullName, data);
                }
            }
            else if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                // If the app isn't running and not navigating to a specific page based on the URI, navigate to the home page
                NavigationService.Navigate(typeof(ViewModels.PivotViewModel).FullName);
            }

            await Task.CompletedTask;
        }

//}]}
    }
}
