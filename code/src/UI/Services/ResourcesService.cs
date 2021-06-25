// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;

namespace Microsoft.Templates.UI.Services
{
    public class ResourcesService
    {
        private static ResourcesService _instance;
        private Window _mainView;

        public static ResourcesService Instance => _instance ?? (_instance = new ResourcesService());

        private ResourcesService()
        {
        }

        public void Initialize(Window mainView)
        {
            _mainView = mainView;
        }

        public T FindResource<T>(object resourceKey)
            where T : class
        {
            return FindResource(resourceKey) as T;
        }

        public object FindResource(object resourceKey) => _mainView.FindResource(resourceKey);
    }
}
