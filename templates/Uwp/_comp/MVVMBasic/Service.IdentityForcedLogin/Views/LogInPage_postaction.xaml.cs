//{[{
using Param_RootNamespace.ViewModels;
//}]}
namespace Param_RootNamespace.Views
{
    public sealed partial class LogInPage : Page
    {
//{[{
        public LogInViewModel ViewModel { get; } = new LogInViewModel();

//}]}
        public LogInPage()
        {
            InitializeComponent();
        }
    }
}
