using ProjectTypeApp.ProjectTypes.Tabbed;
using ProjectTypeApp.ProjectTypes.MasterDetail;

using Xamarin.Forms;

namespace ProjectTypeApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

// WTS: this is for quick testing
// this code wouldn't be here
// talking with pierce, these should be agnostic to CB or MVVM
            var type = ProjectType.FlyoutPage;
            if (type == ProjectType.BlankPage)
            {
                MainPage = new BlankPage();
            }
            else if (type == ProjectType.TabbedPage)
            {
                MainPage = new TabsPage();
            }
            else
            {
                MainPage = new ProjectTypes.MasterDetail.MasterDetailPage();
            }
        }

        enum ProjectType
        {
            BlankPage,
            TabbedPage,
            FlyoutPage // navview style
        }
    }
}
