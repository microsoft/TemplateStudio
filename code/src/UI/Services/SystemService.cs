// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Microsoft.Templates.UI.Threading;

namespace Microsoft.Templates.UI.Services
{
    public class SystemService : DependencyObject
    {
        public static SystemService Current { get; private set; }

        public Visibility DebugComponentVisibility
        {
            get
            {
#if DEBUG
                return Visibility.Visible;
#else
                return Visibility.Collapsed;
#endif
            }
        }

        public SystemService()
        {
            Current = this;
        }

        public void Initialize()
        {
            SystemParameters.StaticPropertyChanged += OnStaticPropertyChanged;
        }

        private async void OnStaticPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
            if (e.PropertyName == "HighContrast")
            {
                IsHighContrast = SystemParameters.HighContrast;
            }
        }

        public static readonly DependencyProperty IsHighContrastProperty = DependencyProperty.Register("IsHighContrast", typeof(bool), typeof(SystemService), new PropertyMetadata(SystemParameters.HighContrast));

        public bool IsHighContrast
        {
            get => (bool)GetValue(IsHighContrastProperty);
            private set => SetValue(IsHighContrastProperty, value);
        }

        public void UnsubscribeEventHandlers()
        {
            SystemParameters.StaticPropertyChanged -= OnStaticPropertyChanged;
        }

        public double GetCodeFontSize()
        {
            double fontSize = 11;
            fontSize = Math.Ceiling(fontSize * VisualTreeHelper.GetDpi(Application.Current.MainWindow as Visual).PixelsPerDip);
            if (fontSize > 25)
            {
                fontSize = 25;
            }
            else if (fontSize < 9)
            {
                fontSize = 9;
            }

            return fontSize;
        }

        public (double width, double height) GetMainWindowSize()
        {
            double width = 1120; // 1277;
            double height = 733; // 727
            double dpi = 0;
            if (Application.Current != null)
            {
                dpi = VisualTreeHelper.GetDpi(Application.Current.MainWindow as Visual).PixelsPerDip;
            }

            if (dpi >= 2)
            {
                return (width / 1.10, height / 1.10);
            }
            else if (dpi >= 1.75)
            {
                return (width / 1.07, height / 1.07);
            }
            else if (dpi >= 1.5)
            {
                return (width / 1.03, height / 1.03);
            }
            else if (dpi >= 1.25)
            {
                return (width / 1.01, height / 1.01);
            }
            else
            {
                return (width, height);
            }
        }
    }
}
