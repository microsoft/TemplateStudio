//{[{
using System.Linq;
using System.Threading.Tasks;
using Param_ItemNamespace.Activation;
//}]}
namespace Param_ItemNamespace.ViewModels
{
    public class PivotViewModel : Conductor<Screen>.Collection.OneActive
    {
//{[{
        public SchemeActivationData ActivationData { get; set; }

//}]}
        protected override void OnInitialize()
        {
            //^^
            //{[{
            Items.Add(new SchemeActivationSampleViewModel { DisplayName = "PivotItem_SchemeActivationSample/Header".GetLocalized() });
            //}]}
        }

//{[{
        protected override async void OnViewReady(object view)
        {
            base.OnViewReady(view);
            if (ActivationData != null)
            {
                await InitializeFromSchemeActivationAsync();
            }
        }

        public async Task InitializeFromSchemeActivationAsync()
        {
            var selectedScreen = Items.FirstOrDefault(s => s.GetView().GetType() == ActivationData.PageType);
            ActivateItem(selectedScreen);
            if (selectedScreen.GetView() is IPivotActivationPage page)
            {
                await page.OnPivotActivatedAsync(ActivationData.Parameters);
            }
        }
//}]}
    }
}
