// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;

namespace Microsoft.Templates.UI.V2Extensions
{
    public static class WindowExtensions
    {
        public static readonly DependencyProperty CenterOnSizeChangeProperty =
            DependencyProperty.RegisterAttached(
                "CenterOnSizeChange",
                typeof(bool),
                typeof(WindowExtensions),
                new UIPropertyMetadata(false, OnCenterOnSizeChangePropertyChanged));

        public static bool GetCenterOnSizeChange(DependencyObject obj)
        {
            return (bool)obj.GetValue(CenterOnSizeChangeProperty);
        }

        public static void SetCenterOnSizeChange(DependencyObject obj, bool value)
        {
            obj.SetValue(CenterOnSizeChangeProperty, value);
        }

        private static void OnCenterOnSizeChangePropertyChanged(DependencyObject dpo, DependencyPropertyChangedEventArgs args)
        {
            if (dpo is Window window)
            {
                if ((bool)args.NewValue)
                {
                    window.SizeChanged += OnWindowSizeChanged;
                }
                else
                {
                    window.SizeChanged -= OnWindowSizeChanged;
                }
            }
        }

        private static void OnWindowSizeChanged(object sender, SizeChangedEventArgs sizeInfo)
        {
            Window window = (Window)sender;

            window.WindowStartupLocation = WindowStartupLocation.Manual;
            window.Left = ((SystemParameters.WorkArea.Width - window.ActualWidth) / 2) + SystemParameters.WorkArea.Left;
            window.Top = ((SystemParameters.WorkArea.Height - window.ActualHeight) / 2) + SystemParameters.WorkArea.Top;
        }
    }
}
