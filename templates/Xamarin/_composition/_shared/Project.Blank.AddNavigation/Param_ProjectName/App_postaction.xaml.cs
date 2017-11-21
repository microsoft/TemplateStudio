//{[{
using Param_RootNamespace.Services;
//}]}
namespace Param_RootNamespace
{
    public partial class App : Application
    {
        public App ()
        {
            InitializeComponent();
            //^^
            //{[{
            RegisterNavigationPages();
            MainPage = new NavigationPage(new Param_HomeNamePage());
            //}]}
        }
        //^^
        //{[{
        private void RegisterNavigationPages()
        {
            var navigationService = NavigationService.Instance;
            
        }
        //}]}
    }
}
