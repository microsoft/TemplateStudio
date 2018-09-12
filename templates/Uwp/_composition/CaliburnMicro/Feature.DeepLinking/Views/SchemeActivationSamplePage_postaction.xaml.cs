//{[{
using Param_ItemNamespace.ViewModels;
//}]}
namespace Param_ItemNamespace.Views
{
    public sealed partial class SchemeActivationSamplePage : Page
    {
//{[{
        private SchemeActivationSampleViewModel ViewModel => DataContext as SchemeActivationSampleViewModel;

//}]}
        public SchemeActivationSamplePage()
        {
            InitializeComponent();
        }
    }
}
