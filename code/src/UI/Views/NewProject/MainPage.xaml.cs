// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.ViewModels.NewProject;

namespace Microsoft.Templates.UI.Views.NewProject
{
    public partial class MainPage : Page
    {
        private IInputElement _focusedElement;

        public MainPage()
        {
            DataContext = MainViewModel.Instance;
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (stepFrame.Content == null)
            {
                Services.NavigationService.InitializeSecondaryFrame(stepFrame, WizardNavigation.Current.CurrentStep.GetPage());
                sequentialFlow.FocusFirstStep();
            }

            if (_focusedElement != null)
            {
                _focusedElement.Focus();
                Keyboard.Focus(_focusedElement);
            }

            Services.NavigationService.SubscribeEventHandlers();
            WizardNavigation.Current.SubscribeEventHandlers();
            PreviewGotKeyboardFocus += OnPreviewGotKeyboardFocus;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Services.NavigationService.UnsubscribeEventHandlers();
            WizardNavigation.Current.UnsubscribeEventHandlers();
            PreviewGotKeyboardFocus -= OnPreviewGotKeyboardFocus;
        }

        private void ComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!(sender is ComboBox comboBox))
            {
                return;
            }

            if (e.Key == Key.Space)
            {
                comboBox.IsDropDownOpen = !comboBox.IsDropDownOpen;
            }

            if (comboBox != null && !comboBox.IsDropDownOpen)
            {
                if (e.Key == Key.Left
                    || e.Key == Key.Up
                    || e.Key == Key.Right
                    || e.Key == Key.Down)
                {
                    e.Handled = true;
                }
            }
        }

        private void UserSelectionGroupLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is ListView listView)
            {
                if (listView.Tag is TemplateType templateType)
                {
                    var group = MainViewModel.Instance.UserSelection.Groups.First(g => g.TemplateType == templateType);
                    group.EnableOrdering(listView);
                }
            }
        }

        private void OnPreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            _focusedElement = e.NewFocus;
        }
    }
}
