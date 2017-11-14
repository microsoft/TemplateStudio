using Xamarin.Forms;

namespace WtsXamarin
{
    public partial class App : Application
    {
        public static NavigationPage NavPage = null;

        public App()
        {
            InitializeComponent();

            NavPage = new NavigationPage(new Views.MainPage());
            MainPage = new Views.Navigation.MasterDetailPage()
            {
                Detail = NavPage
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
