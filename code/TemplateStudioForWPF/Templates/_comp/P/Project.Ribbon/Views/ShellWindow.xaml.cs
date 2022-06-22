using System.Windows;
using Param_RootNamespace.Constants;
using Param_RootNamespace.Contracts.Services;
using Fluent;

using MahApps.Metro.Controls;
using Prism.Regions;

namespace Param_RootNamespace.Views;

public partial class ShellWindow : MetroWindow, IRibbonWindow
{
    public RibbonTitleBar TitleBar
    {
        get => (RibbonTitleBar)GetValue(TitleBarProperty);
        private set => SetValue(TitleBarPropertyKey, value);
    }

    private static readonly DependencyPropertyKey TitleBarPropertyKey = DependencyProperty.RegisterReadOnly(nameof(TitleBar), typeof(RibbonTitleBar), typeof(ShellWindow), new PropertyMetadata());

    public static readonly DependencyProperty TitleBarProperty = TitleBarPropertyKey.DependencyProperty;

    public ShellWindow(IRegionManager regionManager, IRightPaneService rightPaneService)
    {
        InitializeComponent();
        RegionManager.SetRegionName(menuContentControl, Regions.Main);
        RegionManager.SetRegionManager(menuContentControl, regionManager);
        rightPaneService.Initialize(splitView, rightPaneContentControl);
        navigationBehavior.Initialize(regionManager);
        tabsBehavior.Initialize(regionManager);
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        var window = sender as MetroWindow;
        TitleBar = window.FindChild<RibbonTitleBar>("RibbonTitleBar");
        TitleBar.InvalidateArrange();
        TitleBar.UpdateLayout();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        tabsBehavior.Unsubscribe();
    }
}
