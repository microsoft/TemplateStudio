// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Input;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.Styles;
using Microsoft.Templates.UI.ViewModels.NewItem;
using Microsoft.Templates.UI.Views.Common;

namespace Microsoft.Templates.UI.Views.NewItem
{
    public partial class NewItemWizardShell : Window, IWindow, IWizardShell
    {
        private readonly TemplateType _templateType;
        private readonly UserSelectionContext _context;

        public static NewItemWizardShell Current { get; private set; }

        public UserSelection Result { get; set; }

        public MainViewModel ViewModel { get; }

        public NewItemWizardShell(TemplateType templateType, UserSelectionContext context, BaseStyleValuesProvider provider)
        {
            Resources.MergedDictionaries.Add(AllStylesDictionary.GetMergeDictionary());

            _templateType = templateType;
            _context = context;
            ViewModel = new MainViewModel(this, provider);
            Current = this;
            DataContext = ViewModel;
            InitializeComponent();
            NavigationService.InitializeMainFrame(mainFrame, new NewItemMainPage());
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
                && sender is NewItemWizardShell shell
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
            MainViewModel.Instance.Initialize(_templateType, _context);
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
