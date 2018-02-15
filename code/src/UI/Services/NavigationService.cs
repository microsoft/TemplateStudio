// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Controls;

namespace Microsoft.Templates.UI.Services
{
    public static class NavigationService
    {
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
        }

        public static bool NavigateMainFrame(object content) => _mainFrame.Navigate(content);

        public static bool NavigateSecondaryFrame(object content) => _secondaryFrame.Navigate(content);

        public static void GoBackMainFrame() => _mainFrame.GoBack();

        public static void GoBackSecondaryFrame() => _secondaryFrame.GoBack();
    }
}
