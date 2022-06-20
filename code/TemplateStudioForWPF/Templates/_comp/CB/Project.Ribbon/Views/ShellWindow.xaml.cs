using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Fluent;
using MahApps.Metro.Controls;
using Param_RootNamespace.Behaviors;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Contracts.Views;

namespace Param_RootNamespace.Views;

public partial class ShellWindow : MetroWindow, IShellWindow, IRibbonWindow, INotifyPropertyChanged
{
    private readonly IRightPaneService _rightPaneService;

    public RibbonTitleBar TitleBar
    {
        get => (RibbonTitleBar)GetValue(TitleBarProperty);
        private set => SetValue(TitleBarPropertyKey, value);
    }

    private static readonly DependencyPropertyKey TitleBarPropertyKey = DependencyProperty.RegisterReadOnly(nameof(TitleBar), typeof(RibbonTitleBar), typeof(ShellWindow), new PropertyMetadata());

    public static readonly DependencyProperty TitleBarProperty = TitleBarPropertyKey.DependencyProperty;

    public ShellWindow(IServiceProvider serviceProvider, IRightPaneService rightPaneService)
    {
        _rightPaneService = rightPaneService;
        InitializeComponent();
        navigationBehavior.Initialize(serviceProvider);
        DataContext = this;
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

    public void CloseWindow()
        => Close();

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

    public event PropertyChangedEventHandler PropertyChanged;

    private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
    {
        if (Equals(storage, value))
        {
            return;
        }

        storage = value;
        OnPropertyChanged(propertyName);
    }

    private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
