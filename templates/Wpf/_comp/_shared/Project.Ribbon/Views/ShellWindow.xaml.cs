using System;
using System.Windows;
using System.Windows.Controls;
using Fluent;
using MahApps.Metro.Controls;
using Param_RootNamespace.Behaviors;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Contracts.Views;
using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace.Views
{
    public partial class ShellWindow : MetroWindow, IShellWindow
    {
        private RibbonTitleBar _titleBar;

        public ShellWindow(IPageService pageService)
        {
            InitializeComponent();
            navigationBehavior.Initialize(pageService);
        }

        public Frame GetNavigationFrame()
            => shellFrame;

        public RibbonTabsBehavior GetRibbonTabsBehavior()
            => tabsBehavior;

        public Frame GetRightPaneFrame()
            => rightPaneFrame;

        public SplitView GetSplitView()
            => splitView;

        public void ShowWindow()
            => Show();

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var window = sender as MetroWindow;
            _titleBar = window.FindChild<RibbonTitleBar>("RibbonTitleBar");
            _titleBar.InvalidateArrange();
            _titleBar.UpdateLayout();
        }
    }
}
