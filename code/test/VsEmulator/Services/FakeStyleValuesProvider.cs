// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.VsEmulator.Main;

namespace Microsoft.Templates.VsEmulator.Services
{
    public class FakeStyleValuesProvider : IStyleValuesProvider
    {
        private double _baseFontSize = 12;

        public event EventHandler ThemeChanged;

        public Brush GetColor(string className, string memberName)
        {
            var dictionaryName = $"{MainViewModel.ThemeName}_{className}";
            var resourceName = $"{memberName}BrushKey";
            return GetColorFromResourceDictionary(dictionaryName, resourceName);
        }

        public double GetFontSize(string fontSizeResourceKey)
        {
            switch (fontSizeResourceKey)
            {
                case "Environment100PercentFontSize":
                    return _baseFontSize * 1.0;
                case "Environment111PercentFontSize":
                    return _baseFontSize * 1.11;
                case "Environment122PercentFontSize":
                    return _baseFontSize * 1.22;
                case "Environment133PercentFontSize":
                    return _baseFontSize * 1.33;
                case "Environment155PercentFontSize":
                    return _baseFontSize * 1.55;
                case "Environment200PercentFontSize":
                    return _baseFontSize * 2.0;
                case "Environment310PercentFontSize":
                    return _baseFontSize * 3.1;
                case "Environment330PercentFontSize":
                    return _baseFontSize * 3.3;
                case "Environment375PercentFontSize":
                    return _baseFontSize * 3.75;
                case "Environment90PercentFontSize":
                    return _baseFontSize * 0.9;
                default:
                    throw new Exception($"The font size value '{fontSizeResourceKey}' is not found");
            }
        }

        public FontFamily GetFontFamily() => new FontFamily("Segoe UI");

        private void OnThemeChanged()
        {
            ThemeChanged?.Invoke(this, EventArgs.Empty);
        }

        private Brush GetColorFromResourceDictionary(string dictionaryName, string resourceName)
        {
            var colorsDictionary = new ResourceDictionary();
            colorsDictionary.Source = new Uri($"/VsEmulator;component/Styles/{dictionaryName}.xaml", UriKind.RelativeOrAbsolute);
            if (colorsDictionary.Contains(resourceName))
            {
                return colorsDictionary[resourceName] as SolidColorBrush;
            }
            else
            {
                return default(SolidColorBrush);
            }
        }

        public SolidColorBrush GetColor(string resourceKey) => Application.Current.FindResource(resourceKey) as SolidColorBrush;
    }
}
