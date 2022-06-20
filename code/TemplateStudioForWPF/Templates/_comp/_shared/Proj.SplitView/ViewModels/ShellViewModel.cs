﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Properties;

namespace Param_RootNamespace.ViewModels;

public class ShellViewModel : System.ComponentModel.INotifyPropertyChanged
{
    private readonly INavigationService _navigationService;
    private HamburgerMenuItem _selectedMenuItem;
    private System.Windows.Input.ICommand _goBackCommand;
    private ICommand _menuItemInvokedCommand;
    private ICommand _loadedCommand;
    private ICommand _unloadedCommand;

    public HamburgerMenuItem SelectedMenuItem
    {
        get { return _selectedMenuItem; }
        set { Param_Setter(ref _selectedMenuItem, value); }
    }

    // TODO: Change the icons and titles for all HamburgerMenuItems here.
    public ObservableCollection<HamburgerMenuItem> MenuItems { get; } = new ObservableCollection<HamburgerMenuItem>()
    {
    };

    public System.Windows.Input.ICommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new System.Windows.Input.ICommand(OnGoBack, CanGoBack));

    public ICommand MenuItemInvokedCommand => _menuItemInvokedCommand ?? (_menuItemInvokedCommand = new System.Windows.Input.ICommand(OnMenuItemInvoked));

    public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new System.Windows.Input.ICommand(OnLoaded));

    public ICommand UnloadedCommand => _unloadedCommand ?? (_unloadedCommand = new System.Windows.Input.ICommand(OnUnloaded));

    public ShellViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    private void OnLoaded()
    {
        _navigationService.Navigated += OnNavigated;
    }

    private void OnUnloaded()
    {
        _navigationService.Navigated -= OnNavigated;
    }

    private bool CanGoBack()
        => _navigationService.CanGoBack;

    private void OnGoBack()
        => _navigationService.GoBack();

    private void OnMenuItemInvoked()
        => NavigateTo(SelectedMenuItem.TargetPageType);

    private void NavigateTo(Type targetViewModel)
    {
        if (targetViewModel != null)
        {
            _navigationService.NavigateTo(targetViewModel.FullName);
        }
    }

    private void OnNavigated(object sender, string viewModelName)
    {
        var item = MenuItems
                    .OfType<HamburgerMenuItem>()
                    .FirstOrDefault(i => viewModelName == i.TargetPageType?.FullName);
        if (item != null)
        {
            SelectedMenuItem = item;
        }
    }
}
