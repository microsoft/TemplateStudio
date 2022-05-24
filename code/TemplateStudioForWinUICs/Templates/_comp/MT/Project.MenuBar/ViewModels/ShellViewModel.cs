﻿using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;

using Param_RootNamespace.Contracts.Services;

namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ObservableRecipient
    {
        private bool _isBackEnabled;
        private object _selected;
        private ICommand _menuFileExitCommand;

        public ICommand MenuFileExitCommand => _menuFileExitCommand ??= new RelayCommand(OnMenuFileExit);

        public INavigationService NavigationService { get; }

        public bool IsBackEnabled
        {
            get => _isBackEnabled;
            set => SetProperty(ref _isBackEnabled, value);
        }

        public object Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }

        public ShellViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            NavigationService.Navigated += OnNavigated;
        }

        private void OnNavigated(object sender, NavigationEventArgs e) => IsBackEnabled = NavigationService.CanGoBack;

        private void OnMenuFileExit() => Application.Current.Exit();
    }
}
