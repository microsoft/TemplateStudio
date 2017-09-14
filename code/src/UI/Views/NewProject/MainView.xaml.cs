// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.ViewModels.NewProject;

namespace Microsoft.Templates.UI.Views.NewProject
{
    public partial class MainView : Window
    {
        public static MainView Current;
        public MainViewModel ViewModel { get; private set; }
        public UserSelection Result { get; set; }

        public MainView()
        {
            Current = this;
            ViewModel = new MainViewModel(this);
            DataContext = ViewModel;

            Loaded += async (sender, e) =>
            {
                await ViewModel.InitializeAsync(stepFrame, summaryPageGroups);
            };

            Unloaded += (sender, e) =>
            {
                ViewModel.UnsuscribeEventHandlers();
            };

            InitializeComponent();
        }

        private void OnPreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var textBox = e.OldFocus as TextBoxEx;
            var button = e.NewFocus as Button;
            ViewModel.TryCloseEdition(textBox, button);
        }

        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var element = e.Source as FrameworkElement;
            ViewModel.WizardStatus.TryHideOverlayBox(element);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (!ViewModel.ProjectTemplates.CloseAllEditions() && !ViewModel.Ordering.ClearDraggin())
                {
                    Close();
                }
            }
        }
    }
}
