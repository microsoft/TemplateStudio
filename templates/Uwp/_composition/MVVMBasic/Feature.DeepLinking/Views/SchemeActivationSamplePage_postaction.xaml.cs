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
//{[{

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var parameters = e.Parameter as Dictionary<string, string>;
            if (parameters != null)
            {
                ViewModel.Initialize(parameters);
            }
        }
//}]}
    }
}
