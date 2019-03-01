// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Input;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.NewProject;

namespace Microsoft.Templates.UI.Views.NewProject
{
    public partial class InvalidProjectName : Window, IWindow
    {
        private string _language;
        private string _platform;

        public MainViewModel ViewModel { get; }

        public InvalidProjectName(string platform, string language, BaseStyleValuesProvider provider)
        {
            _platform = platform;
            _language = language;
            ViewModel = new MainViewModel(this, provider)
            {
                CloseWindow = () => Close(),
            };
            DataContext = ViewModel;
            InitializeComponent();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
                return;
            }
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await MainViewModel.Instance.InitializeAsync(_platform, _language);
        }
    }
}
