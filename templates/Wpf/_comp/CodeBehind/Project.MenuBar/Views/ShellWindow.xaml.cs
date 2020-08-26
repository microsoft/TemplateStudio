using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Contracts.Views;
using MahApps.Metro.Controls;

namespace Param_RootNamespace.Views
{
    public partial class ShellWindow : MetroWindow, IShellWindow, INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private readonly IRightPaneService _rightPaneService;
        private bool _canGoBack;

        public bool CanGoBack
        {
            get { return _canGoBack; }
            set { Set(ref _canGoBack, value); }
        }

        public ShellWindow(INavigationService navigationService, IRightPaneService rightPaneService)
        {
            _navigationService = navigationService;
            _rightPaneService = rightPaneService;
            InitializeComponent();
            DataContext = this;
        }

        public Frame GetNavigationFrame()
            => shellFrame;

        public Frame GetRightPaneFrame()
            => rightPaneFrame;

        public void ShowWindow()
            => Show();

        public void CloseWindow()
            => Close();

        public SplitView GetSplitView()
            => splitView;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _navigationService.Navigated += OnNavigated;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _navigationService.Navigated -= OnNavigated;
        }

        private void OnGoBack(object sender, RoutedEventArgs e)
        {
            _navigationService.GoBack();
        }

        private void OnNavigated(object sender, Type pageType)
        {
            CanGoBack = _navigationService.CanGoBack;
        }

        private void OnMenuFileExit(object sender, RoutedEventArgs e)
            => Application.Current.Shutdown();

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
}
