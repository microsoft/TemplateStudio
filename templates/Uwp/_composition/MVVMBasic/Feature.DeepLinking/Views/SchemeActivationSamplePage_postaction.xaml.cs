//{[{
using Param_ItemNamespace.ViewModels;
//}]}
namespace Param_ItemNamespace.Views
{
    public sealed partial class SchemeActivationSamplePage : Page
    {
//{[{
        public SchemeActivationSampleViewModel ViewModel { get; } = new SchemeActivationSampleViewModel();

//}]}
        public SchemeActivationSamplePage()
        {
            InitializeComponent();
        }
    }
}
