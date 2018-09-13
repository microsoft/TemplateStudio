//{[{
using System.Threading.Tasks;
using Param_ItemNamespace.Helpers;
//}]}
namespace Param_ItemNamespace.Views
{
        public SchemeActivationSamplePage()
        {
            InitializeComponent();
        }
//{[{

        public async Task OnPivotActivatedAsync(Dictionary<string, string> parameters)
        {
            Initialize(parameters);
            await Task.CompletedTask;
        }
//}]}
}
