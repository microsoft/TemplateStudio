using Xamarin.Forms;
using wts.DefaultProject.Views;

namespace wts.DefaultProject
{
    public partial class App : Application
	{
        public static NavigationPage NavPage = null;

        public App ()
		{
			InitializeComponent();

            //FIRST PAGE IS REQUIRED TO START THE APP
            NavPage = new NavigationPage(new FirstPage());

            //DEFAULT
            MainPage = NavPage;

            //MASTER DETAIL
            //MainPage = new Views.Navigation.MasterDetailPage()
            //{
            //    Detail = NavPage
            //};
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
