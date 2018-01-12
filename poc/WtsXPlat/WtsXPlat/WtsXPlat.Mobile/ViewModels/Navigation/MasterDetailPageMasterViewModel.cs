using System.Collections.ObjectModel;
using WtsXPlat.Core.Helpers;
using WtsXPlat.Mobile.Views;
using WtsXPlat.Mobile.Views.Navigation;

namespace WtsXPlat.Mobile.ViewModels.Navigation
{
    public class MasterDetailPageMasterViewModel : Observable
    {
        public MasterDetailPageMasterViewModel()
        {
        }

        public ObservableCollection<MasterDetailPageMenuItem> PrimaryMenuItems { get; private set; } = new ObservableCollection<MasterDetailPageMenuItem>
        {
            new MasterDetailPageMenuItem { Id = 0, Title = "Main", TargetType = typeof(MainPage), IconSource = "blank.png"},
            new MasterDetailPageMenuItem { Id = 1, Title = "Blank", TargetType = typeof(BlankPage), IconSource = "blank.png"},
            new MasterDetailPageMenuItem { Id = 2, Title = "WebView", TargetType = typeof(WebViewPage), IconSource = "blank.png"},
            new MasterDetailPageMenuItem { Id = 3, Title = "ListView", TargetType = typeof(ListViewPage), IconSource = "blank.png"},
            new MasterDetailPageMenuItem { Id = 4, Title = "Camera", TargetType = typeof(CameraPage), IconSource = "blank.png"},
            new MasterDetailPageMenuItem { Id = 6, Title = "ListView List", TargetType = typeof(ListViewListPage), IconSource = "blank.png"},
            new MasterDetailPageMenuItem { Id = 99, Title = "Settings", TargetType = typeof(SettingsPage), IconSource = "settings.png" },
        };
    }
}
