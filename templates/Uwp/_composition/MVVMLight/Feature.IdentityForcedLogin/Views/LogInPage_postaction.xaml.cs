//{[{
using Param_RootNamespace.ViewModels;
//}]}
namespace Param_RootNamespace.Views
{
    public sealed partial class LogInPage : Page
    {
//{[{
        private LogInViewModel ViewModel
        {
            get { return ViewModelLocator.Current.LogInViewModel; }
        }

//}]}
        public LogInPage()
        {
            InitializeComponent();
        }
    }
}
