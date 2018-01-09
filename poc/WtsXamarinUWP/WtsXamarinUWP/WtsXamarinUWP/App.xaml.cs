using WtsXamarinUWP.Mobile.Services;
using WtsXamarinUWP.Mobile.ViewModels;
using WtsXamarinUWP.Mobile.Views;
using Xamarin.Forms;

namespace WtsXamarinUWP.Mobile
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();
            RegisterNavigationPages();
            MainPage = new Views.Navigation.MasterDetailPage();
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

        private void RegisterNavigationPages()
        {
            var navigationService = NavigationService.Instance;

            navigationService.Register<MainViewModel>(typeof(MainPage));
            navigationService.Register<BlankViewModel>(typeof(BlankPage));
            navigationService.Register<WebViewViewModel>(typeof(WebViewPage));
            navigationService.Register<ListViewViewModel>(typeof(ListViewPage));
            navigationService.Register<CameraViewModel>(typeof(CameraPage));
            navigationService.Register<ListViewListViewModel>(typeof(ListViewListPage));
            navigationService.Register<ListViewItemViewModel>(typeof(ListViewItemPage));
            navigationService.Register<SettingsViewModel>(typeof(SettingsPage));
        }
    }
}
