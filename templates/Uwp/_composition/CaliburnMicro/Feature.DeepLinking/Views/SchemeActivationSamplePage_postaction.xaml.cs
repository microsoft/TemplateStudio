//{[{
using Param_RootNamespace.ViewModels;
//}]}
namespace Param_RootNamespace.Views
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
