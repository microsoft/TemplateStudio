// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Input;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.NewItem;
using Microsoft.Templates.UI.Views.Common;

namespace Microsoft.Templates.UI.Views.NewItem
{
    public partial class WizardShell : Window, IWindow, IWizardShell
    {
        private TemplateType _templateType;
        private string _language;

        public static WizardShell Current { get; private set; }

        public UserSelection Result { get; set; }

        public MainViewModel ViewModel { get; }

        public WizardShell(TemplateType templateType, string language, BaseStyleValuesProvider provider)
        {
            _templateType = templateType;
            _language = language;
            ViewModel = new MainViewModel(this, provider);
            Current = this;
            DataContext = ViewModel;
            InitializeComponent();
            NavigationService.InitializeMainFrame(mainFrame, new MainPage());
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
                return;
            }

            if (e.Key == Key.Back
                && NavigationService.CanGoBackMainFrame
                && sender is WizardShell shell
                && shell.mainFrame.NavigationService.Content is TemplateInfoPage)
            {
                NavigationService.GoBackMainFrame();
            }
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnMouseLeftButtonDown(e);
            DragMove();
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            MainViewModel.Instance.Initialize(_templateType, _language);
            await MainViewModel.Instance.SynchronizeAsync();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.UnsubscribeEventHandlers();
            NotificationsControl.UnsubscribeEventHandlers();
        }
    }
}
