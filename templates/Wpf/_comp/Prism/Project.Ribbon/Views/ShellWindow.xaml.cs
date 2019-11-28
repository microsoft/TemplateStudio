using System.Windows;
using Param_RootNamespace.Constants;
using Param_RootNamespace.Contracts.Services;
using Fluent;

using MahApps.Metro.Controls;
using Prism.Regions;

namespace Param_RootNamespace.Views
{
    public partial class ShellWindow : MetroWindow
    {
        private RibbonTitleBar _titleBar;

        public ShellWindow(IRegionManager regionManager, IRightPaneService rightPaneService)
        {
            InitializeComponent();
            RegionManager.SetRegionName(menuContentControl, Regions.Main);
            RegionManager.SetRegionManager(menuContentControl, regionManager);
            rightPaneService.Initialize(splitView, rightPaneContentControl);
            navigationBehavior.Initialize(regionManager);
            tabsBehavior.Initialize(regionManager);
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
