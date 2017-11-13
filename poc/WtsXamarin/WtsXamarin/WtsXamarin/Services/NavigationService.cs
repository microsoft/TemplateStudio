using System.Collections.Generic;
using WtsXamarin.Views;
using WtsXamarin.Views.Navigation;

namespace WtsXamarin.Services
{
    public static class NavigationService
    {
        public static IEnumerable<MasterDetailPageMenuItem> PrimaryNavigationItems = new[]
        {
            new MasterDetailPageMenuItem { Id = 0, Title = "Main", TargetType = typeof(MainPage), IconSource = "blank.png"},
            new MasterDetailPageMenuItem { Id = 1, Title = "Blank", TargetType = typeof(BlankPage), IconSource = "blank.png"},
            new MasterDetailPageMenuItem { Id = 2, Title = "WebView", TargetType = typeof(WebViewPage), IconSource = "blank.png"},
            new MasterDetailPageMenuItem { Id = 3, Title = "ListView", TargetType = typeof(ListViewPage), IconSource = "blank.png"},
            new MasterDetailPageMenuItem { Id = 4, Title = "Camera", TargetType = typeof(CameraPage), IconSource = "blank.png"},
            new MasterDetailPageMenuItem { Id = 99, Title = "Settings", TargetType = typeof(SettingsPage), IconSource = "settings.png" },
        };
    }
}
