// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.Services
{
    public static class NavigationService
    {
        private static bool _updateStep = false;
        private static Frame _mainFrame;
        private static Frame _secondaryFrame;

        public static bool CanGoBackMainFrame => _mainFrame.CanGoBack;

        public static bool CanGoBackSecondaryFrame => _secondaryFrame.CanGoBack;

        public static void InitializeMainFrame(Frame mainFrame, object content)
        {
            _mainFrame = mainFrame;
            _mainFrame.Content = content;
        }

        public static void InitializeSecondaryFrame(Frame secondaryFrame, object content)
        {
            _secondaryFrame = secondaryFrame;
            _secondaryFrame.Content = content;
            _secondaryFrame.Navigated += SecondaryFrameNavigated;
            _secondaryFrame.Navigating += SecondaryFrameNavigating;
        }

        public static void UnsubscribeEventHandlers()
        {
            _secondaryFrame.Navigated -= SecondaryFrameNavigated;
            _secondaryFrame.Navigating -= SecondaryFrameNavigating;
        }

        private static void SecondaryFrameNavigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back || e.NavigationMode == NavigationMode.Forward)
            {
                _updateStep = true;
            }
        }

        private static void SecondaryFrameNavigated(object sender, NavigationEventArgs e)
        {
            if (_updateStep)
            {
                WizardNavigation.Current.RefreshStep(e.Content);
                _updateStep = false;
            }
        }

        public static bool NavigateMainFrame(object content) => _mainFrame.Navigate(content);

        public static void NavigateSecondaryFrame(object content) => _secondaryFrame?.Navigate(content);

        public static void GoBackMainFrame() => _mainFrame.GoBack();

        public static void GoBackSecondaryFrame() => _secondaryFrame.GoBack();
    }
}
