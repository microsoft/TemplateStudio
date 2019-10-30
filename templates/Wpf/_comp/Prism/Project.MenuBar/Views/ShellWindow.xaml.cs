using MahApps.Metro.Controls;
using Param_RootNamespace.Constants;
using Prism.Regions;

namespace Param_RootNamespace.Views
{
    public partial class ShellWindow : MetroWindow
    {
        public ShellWindow(IRegionManager regionManager)
        {
            InitializeComponent();
            RegionManager.SetRegionName(menuContentControl, Regions.Main);
            RegionManager.SetRegionManager(menuContentControl, regionManager);
            RegionManager.SetRegionName(rightPaneContentControl, Regions.RightPane);
            RegionManager.SetRegionManager(rightPaneContentControl, regionManager);
            var rightPanenavigationService = regionManager.Regions[Regions.RightPane].NavigationService;
            rightPanenavigationService.Navigated += OnRightPaneNavigated;
        }

        private void OnRightPaneNavigated(object sender, RegionNavigationEventArgs e)
        {
            splitView.IsPaneOpen = true;
        }
    }
}
