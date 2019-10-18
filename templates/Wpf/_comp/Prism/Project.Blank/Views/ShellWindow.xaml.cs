using MahApps.Metro.Controls;
using Prism.Regions;
using Param_RootNamespace.Constants;

namespace Param_RootNamespace.Views
{
    public partial class ShellWindow : MetroWindow
    {
        public ShellWindow(IRegionManager regionManager)
        {
            InitializeComponent();
            RegionManager.SetRegionName(shellContentControl, Regions.Main);
            RegionManager.SetRegionManager(shellContentControl, regionManager);
        }
    }
}
