using System.Windows;
using Param_RootNamespace.Constants;
using Fluent;

using MahApps.Metro.Controls;
using Prism.Regions;

namespace Param_RootNamespace.Views
{
    public partial class ShellWindow : MetroWindow
    {
        private RibbonTitleBar _titleBar;

        public ShellWindow(IRegionManager regionManager)
        {
            InitializeComponent();
            RegionManager.SetRegionName(menuContentControl, Regions.Main);
            RegionManager.SetRegionManager(menuContentControl, regionManager);
            RegionManager.SetRegionName(rightPaneContentControl, Regions.RightPane);
            RegionManager.SetRegionManager(rightPaneContentControl, regionManager);
            var rightPanenavigationService = regionManager.Regions[Regions.RightPane].NavigationService;
            rightPanenavigationService.Navigated += OnRightPaneNavigated;
            navigationBehavior.Initialize(regionManager);
            tabsBehavior.Initialize(regionManager);
        }

        private void OnRightPaneNavigated(object sender, RegionNavigationEventArgs e)
        {
            splitView.IsPaneOpen = true;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var window = sender as MetroWindow;
            _titleBar = window.FindChild<RibbonTitleBar>("RibbonTitleBar");
            _titleBar.InvalidateArrange();
            _titleBar.UpdateLayout();
        }
    }
}
