using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using MahApps.Metro.Controls;
using Param_RootNamespace.Contracts.Views;
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Helpers;
using Param_RootNamespace.Properties;

namespace Param_RootNamespace.Views
{
    public partial class LogInWindow : MetroWindow, ILogInWindow, INotifyPropertyChanged
    {
        private readonly IIdentityService _identityService;
        private string _statusMessage;
        private bool _isBusy;

        public string StatusMessage
        {
            get => _statusMessage;
            set => Set(ref _statusMessage, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => Set(ref _isBusy, value);
        }

        public LogInWindow(IIdentityService identityService)
        {
            _identityService = identityService;
            InitializeComponent();
            DataContext = this;
        }

        public void ShowWindow()
            => Show();

        public void CloseWindow()
            => Close();

        private async void OnLogin(object sender, RoutedEventArgs e)
        {
            IsBusy = true;
            StatusMessage = string.Empty;
            var loginResult = await _identityService.LoginAsync();
            StatusMessage = GetStatusMessage(loginResult);
            IsBusy = false;
        }

        private string GetStatusMessage(LoginResultType loginResult)
        {
            switch (loginResult)
            {
                case LoginResultType.Unauthorized:
                    return Properties.Resources.StatusUnauthorized;
                case LoginResultType.NoNetworkAvailable:
                    return Properties.Resources.StatusNoNetworkAvailable;
                case LoginResultType.UnknownError:
                    return Properties.Resources.StatusLoginFails;
                case LoginResultType.Success:
                case LoginResultType.CancelledByUser:
                    return string.Empty;
                default:
                    return string.Empty;
            }
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
}
