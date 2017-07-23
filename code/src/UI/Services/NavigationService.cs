// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Controls;

namespace Microsoft.Templates.UI.Services
{
    public static class NavigationService
    {
        private static Frame _frame;

        public static void Initialize(Frame frame, object content)
        {
            _frame = frame;
            _frame.Content = content;
        }

        public static void Navigate(object content)
        {
            _frame.Navigate(content);
        }

        public static void GoBack()
        {
            if (_frame.CanGoBack)
            {
                _frame.GoBack();
            }
        }
    }
}
