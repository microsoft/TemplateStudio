// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using Microsoft.Templates.UI.Services;

namespace Microsoft.UI.Test
{
    public class UITestStyleValuesProvider : BaseStyleValuesProvider
    {
        public override event EventHandler ThemeChanged;

        public override System.Windows.Media.Brush GetColor(string className, string memberName)
        {
            return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        public override System.Windows.Media.FontFamily GetFontFamily()
        {
            return new System.Windows.Media.FontFamily("Segoe UI");
        }

        public override double GetFontSize(string fontSizeResourceKey)
        {
            return 12.0;
        }

        public override Style GetStyle(object resourceKey)
        {
            return new Style();
        }

        public override System.Drawing.Color GetThemedColor(string className, string memberName)
        {
            return System.Drawing.Color.FromArgb(255, System.Drawing.Color.Black);
        }

        private void OnThemeChanged()
        {
            ThemeChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
