using Param_RootNamespace.Constants;
using Param_RootNamespace.Contracts.Services;
using MahApps.Metro.Controls;

using Prism.Regions;

namespace Param_RootNamespace.Views
{
    public partial class ShellWindow : MetroWindow
    {
        public ShellWindow(IRegionManager regionManager, IRightPaneService rightPaneService)
        {
            InitializeComponent();
            RegionManager.SetRegionName(menuContentControl, Regions.Main);
            RegionManager.SetRegionManager(menuContentControl, regionManager);
            rightPaneService.Initialize(splitView, rightPaneContentControl);
        }
    }
}
