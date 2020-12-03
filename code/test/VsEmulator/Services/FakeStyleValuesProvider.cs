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
    public class FakeStyleValuesProvider : BaseStyleValuesProvider
    {
        private static FakeStyleValuesProvider _instance;

        private ResourceDictionary _commonControls;
        private ResourceDictionary _commonDocument;
        private ResourceDictionary _environment;
        private ResourceDictionary _infoBar;
        private ResourceDictionary _windowsTemplateStudio;
        private ResourceDictionary _styles;

        public static FakeStyleValuesProvider Instance => _instance ?? (_instance = new FakeStyleValuesProvider());

        private FakeStyleValuesProvider()
        {
        }

        private double _baseFontSize = 12;

        public override event EventHandler ThemeChanged;

        public override Brush GetColor(string className, string memberName)
        {
            switch (className)
            {
                case "CommonControls":
                    return GetColorFromResourceDictionary(_commonControls, $"{memberName}BrushKey");
                case "CommonDocument":
                    return GetColorFromResourceDictionary(_commonDocument, $"{memberName}BrushKey");
                case "Environment":
                    return GetColorFromResourceDictionary(_environment, $"{memberName}BrushKey");
                case "InfoBar":
                    return GetColorFromResourceDictionary(_infoBar, $"{memberName}BrushKey");
                case "WindowsTemplateStudio":
                    return GetColorFromResourceDictionary(_windowsTemplateStudio, $"{memberName}BrushKey");
                default:
                    throw new Exception($"The class name '{className}' is not recognized");
            }
        }

        public override System.Drawing.Color GetThemedColor(string className, string memberName)
        {
            var color = GetColor(className, memberName) as SolidColorBrush;
            return System.Drawing.Color.FromArgb(color.Color.A, color.Color.R, color.Color.G, color.Color.B);
        }

        public override double GetFontSize(string fontSizeResourceKey)
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
                default:
                    throw new Exception($"The font size value '{fontSizeResourceKey}' is not found");
            }
        }

        public override FontFamily GetFontFamily() => new FontFamily("Segoe UI");

        public void LoadResources(string themeName)
        {
            _commonControls = new ResourceDictionary()
            {
                Source = new Uri($"/VsEmulator;component/Styles/{themeName}_CommonControls.xaml", UriKind.RelativeOrAbsolute),
            };
            _commonDocument = new ResourceDictionary()
            {
                Source = new Uri($"/VsEmulator;component/Styles/{themeName}_CommonDocument.xaml", UriKind.RelativeOrAbsolute),
            };
            _environment = new ResourceDictionary()
            {
                Source = new Uri($"/VsEmulator;component/Styles/{themeName}_Environment.xaml", UriKind.RelativeOrAbsolute),
            };
            _infoBar = new ResourceDictionary()
            {
                Source = new Uri($"/VsEmulator;component/Styles/{themeName}_InfoBar.xaml", UriKind.RelativeOrAbsolute),
            };
            _windowsTemplateStudio = new ResourceDictionary()
            {
                Source = new Uri($"/VsEmulator;component/Styles/{themeName}_WindowsTemplateStudio.xaml", UriKind.RelativeOrAbsolute),
            };

            _styles = new ResourceDictionary()
            {
                Source = new Uri($"/VsEmulator;component/Styles/{themeName}_Styles.xaml", UriKind.RelativeOrAbsolute),
            };
        }

        private void OnThemeChanged()
        {
            ThemeChanged?.Invoke(this, EventArgs.Empty);
        }

        private Brush GetColorFromResourceDictionary(ResourceDictionary dictionary, string resourceName)
        {
            if (dictionary.Contains(resourceName))
            {
                return dictionary[resourceName] as SolidColorBrush;
            }
            else
            {
                throw new Exception($"The member name '{resourceName}' is not recognized");
            }
        }

        public SolidColorBrush GetColor(string resourceKey) => Application.Current.FindResource(resourceKey) as SolidColorBrush;

        public override Style GetStyle(object resourceKey)
        {
            if (_styles.Contains(resourceKey))
            {
                return _styles[resourceKey] as Style;
            }
            else
            {
                throw new Exception($"The member name '{resourceKey}' is not recognized");
            }
        }
    }
}
