// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Windows;

namespace Microsoft.Templates.UI.Services
{
    public class SystemService : DependencyObject
    {
        private static SystemService _instance;
        public static SystemService Instance => _instance ?? (_instance = new SystemService());

        private SystemService()
        {
            SystemParameters.StaticPropertyChanged += OnStaticPropertyChanged;
        }

        private void OnStaticPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "HighContrast")
            {
                Instance.IsHighContrast = SystemParameters.HighContrast;
            }
        }

        public static readonly DependencyProperty IsHighContrastProperty = DependencyProperty.Register("IsHighContrast", typeof(bool), typeof(SystemService), new PropertyMetadata(SystemParameters.HighContrast));
        public bool IsHighContrast
        {
            get => (bool)GetValue(IsHighContrastProperty);
            private set => SetValue(IsHighContrastProperty, value);
        }
    }
}
