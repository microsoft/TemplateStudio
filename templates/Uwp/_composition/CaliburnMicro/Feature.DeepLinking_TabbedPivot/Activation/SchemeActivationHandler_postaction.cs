//{[{
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Caliburn.Micro;
using Param_ItemNamespace.Views;
using Param_ItemNamespace.ViewModels;
//}]}
namespace Param_ItemNamespace.Activation
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
                var frame = Window.Current.Content as Frame;
                if (frame.Content is PivotPage pivotPage && pivotPage.DataContext is PivotViewModel viewModel)
                {
                    viewModel.ActivationData = data;
                    await viewModel.InitializeFromSchemeActivationAsync();
                }
                else
                {
                    _navigationService.Navigate(typeof(PivotPage), data);
                }
            }
            else if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                // If the app isn't running and not navigating to a specific page based on the URI, navigate to the home page
                _navigationService.For<PivotViewModel>().Navigate();
            }

            await Task.CompletedTask;
        }

//}]}
    }
}
