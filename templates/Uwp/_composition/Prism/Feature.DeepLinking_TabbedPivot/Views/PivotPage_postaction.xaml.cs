//{[{
using System.Linq;
using System.Threading.Tasks;
using Prism.Windows.Navigation;
using Param_ItemNamespace.Activation;
using Param_ItemNamespace.Helpers;
//}]}
namespace Param_ItemNamespace.Views
{
    public sealed partial class PivotPage : Page
    {
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
//{[{
            if (e.Parameter is SchemeActivationData data)
            {
                InitializeFromSchemeActivation(data);
            }

//}]}
        }

//{[{    
        public void InitializeFromSchemeActivation(SchemeActivationData schemeActivationData)
        {
            var selected = pivot.Items.Cast<PivotItem>()
                    .FirstOrDefault(i => i.IsOfPageType(schemeActivationData.PageToken));
            pivot.SelectedItem = selected;

            var viewModel = selected?.GetPage<INavigationAware>();
            var args = new NavigatedToEventArgs();
            args.Parameter = schemeActivationData.Parameters;
            viewModel.OnNavigatedTo(args, null);
        }
//}]}
    }
}
