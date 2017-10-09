// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;

namespace Microsoft.Templates.UI.Services
{
    public static class ResourceService
    {
        private static Window _mainView;

        public static void Initialize(Window mainWindow)
        {
            _mainView = mainWindow;
        }

        public static T FindResource<T>(string resourceKey) where T : class
        {
            if (_mainView != null)
            {
                return _mainView.FindResource(resourceKey) as T;
            }
            return default(T);
        }
    }
}
