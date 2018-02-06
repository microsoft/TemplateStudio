// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Input;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.V2Services;
using Microsoft.Templates.UI.V2ViewModels.NewItem;

namespace Microsoft.Templates.UI.V2Views.NewItem
{
    public partial class WizardShell : Window
    {
        public static WizardShell Current { get; private set; }

        public UserSelection Result { get; set; }

        public MainViewModel ViewModel { get; }

        public WizardShell(TemplateType templateType, string language)
        {
            ViewModel = new MainViewModel(this);
            Current = this;
            DataContext = ViewModel;
            InitializeComponent();
            NavigationService.InitializeMainFrame(mainFrame, new MainPage());
            Loaded += async (sender, args) =>
            {
                await MainViewModel.Instance.InitializeAsync(templateType, language);
            };

            Unloaded += (sender, e) =>
            {
                ViewModel.UnsuscribeEventHandlers();
            };
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnMouseLeftButtonDown(e);
            DragMove();
        }
    }
}
