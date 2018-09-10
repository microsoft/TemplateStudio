//{[{
using Param_ItemNamespace.ViewModels;
//}]}
namespace Param_ItemNamespace.Views
{
    public sealed partial class SchemeActivationSamplePage : Page
    {
//{[{
        private SchemeActivationSampleViewModel ViewModel
        {
            get { return DataContext as SchemeActivationSampleViewModel; }
        }

//}]}
        public SchemeActivationSamplePage()
        {
            InitializeComponent();
        }
    }
}
