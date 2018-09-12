//{[{
using Param_ItemNamespace.Activation;
//}]}
namespace Param_ItemNamespace.Views
{
    public sealed partial class PivotPage : Page
    {
//{[{
        private PivotViewModel ViewModel => DataContext as PivotViewModel;
//}]}

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
//{[{
            if (e.Parameter is SchemeActivationData activationData)
            {
                ViewModel.ActivationData = activationData;
            }

//}]}
        }
    }
}
