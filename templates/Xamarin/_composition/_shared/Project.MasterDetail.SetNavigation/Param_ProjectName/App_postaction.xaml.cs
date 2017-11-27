namespace Param_RootNamespace
{
    public partial class App : Application
    {
        public App ()
        {
            InitializeComponent();
            //^^
            //{[{
            MainPage = new Views.Navigation.MasterDetailPage();
            //}]}
        }
    }
}
