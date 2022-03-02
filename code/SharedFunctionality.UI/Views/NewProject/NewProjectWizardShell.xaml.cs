// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Input;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.Styles;
using Microsoft.Templates.UI.ViewModels.NewProject;
using Microsoft.Templates.UI.Views.Common;

namespace Microsoft.Templates.UI.Views.NewProject
{
    public partial class NewProjectWizardShell : Window, IWindow, IWizardShell
    {
        private readonly UserSelectionContext _context;

        public static NewProjectWizardShell Current { get; private set; }

        public UserSelection Result { get; set; }

        public MainViewModel ViewModel { get; }

        public NewProjectWizardShell(UserSelectionContext context, BaseStyleValuesProvider provider)
        {
            this.Resources.MergedDictionaries.Add(AllStylesDictionary.GetMergeDictionary());

            _context = context;
            Current = this;
            ViewModel = new MainViewModel(this, provider);
            DataContext = ViewModel;
            InitializeComponent();
            NavigationService.InitializeMainFrame(mainFrame, new NewProjectMainPage());
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
                && sender is NewProjectWizardShell shell
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

#pragma warning disable VSTHRD100 // Avoid async void methods
        private async void OnLoaded(object sender, RoutedEventArgs e)
#pragma warning restore VSTHRD100 // Avoid async void methods
        {
            MainViewModel.Instance.Initialize(_context);
            ////await MainViewModel.Instance.SynchronizeAsync();
            await MainViewModel.Instance.OnTemplatesAvailableAsync();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.UnsubscribeEventHandlers();
            NotificationsControl.UnsubscribeEventHandlers();
        }
    }
}
