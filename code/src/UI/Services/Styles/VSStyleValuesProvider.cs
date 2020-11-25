// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using System.Windows.Media;

using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.Templates.UI.Services
{
    public class VSStyleValuesProvider : BaseStyleValuesProvider
    {
        public VSStyleValuesProvider()
        {
            VSColorTheme.ThemeChanged += OnThemeChanged;
        }

        private void OnThemeChanged(ThemeChangedEventArgs e)
        {
            ThemeChanged?.Invoke(this, EventArgs.Empty);
        }

        public override void UnsubscribeEventHandlers()
        {
            base.UnsubscribeEventHandlers();
            VSColorTheme.ThemeChanged -= OnThemeChanged;
        }

        public override event EventHandler ThemeChanged;

        public override Brush GetColor(string className, string memberName)
        {
            var color = GetThemedColor(className, memberName);
            return new SolidColorBrush(new Color()
            {
                A = color.A,
                R = color.R,
                G = color.G,
                B = color.B,
            });
        }

        public override System.Drawing.Color GetThemedColor(string className, string memberName)
        {
            Type classType = null;
            switch (className)
            {
                case "CommonDocument":
                    classType = typeof(CommonDocumentColors);
                    break;
                case "CommonControls":
                    classType = typeof(CommonControlsColors);
                    break;
                case "Environment":
                    classType = typeof(EnvironmentColors);
                    break;
                case "InfoBar":
                    classType = typeof(InfoBarColors);
                    break;
                case "WindowsTemplateStudio":
                    classType = typeof(WindowsTemplateStudioColors);
                    break;
                default:
                    throw new Exception($"The class name '{className}' is not recognized");
            }

            var member = classType.GetProperty($"{memberName}BrushKey");
            if (member == null)
            {
                throw new Exception($"The member name '{memberName}' is not recognized");
            }

            var themeResourceKey = member.GetMethod.Invoke(null, null) as ThemeResourceKey;
            var themeColor = VSColorTheme.GetThemedColor(themeResourceKey);
            return themeColor;
        }

        public override double GetFontSize(string fontSizeResourceKey)
        {
            switch (fontSizeResourceKey)
            {
                case "Environment100PercentFontSize":
                    return GetVSFontSize(VsFonts.EnvironmentFontSizeKey);
                case "Environment111PercentFontSize":
                    return GetVSFontSize(VsFonts.Environment111PercentFontSizeKey);
                case "Environment122PercentFontSize":
                    return GetVSFontSize(VsFonts.Environment122PercentFontSizeKey);
                case "Environment133PercentFontSize":
                    return GetVSFontSize(VsFonts.Environment133PercentFontSizeKey);
                case "Environment155PercentFontSize":
                    return GetVSFontSize(VsFonts.Environment155PercentFontSizeKey);
                case "Environment200PercentFontSize":
                    return GetVSFontSize(VsFonts.Environment200PercentFontSizeKey);
                case "Environment310PercentFontSize":
                    return GetVSFontSize(VsFonts.Environment310PercentFontSizeKey);
                case "Environment330PercentFontSize":
                    return GetVSFontSize(VsFonts.Environment330PercentFontSizeKey);
                case "Environment375PercentFontSize":
                    return GetVSFontSize(VsFonts.Environment375PercentFontSizeKey);
                default:
                    throw new Exception($"The font size value '{fontSizeResourceKey}' is not found");
            }
        }

        public override FontFamily GetFontFamily()
        {
            var fontFamily = Application.Current.FindResource(VsFonts.EnvironmentFontFamilyKey);
            if (fontFamily is FontFamily)
            {
                return fontFamily as FontFamily;
            }

            throw new Exception($"The font family is not valid.");
        }

        private double GetVSFontSize(object value)
        {
            var font = Application.Current.FindResource(value);
            if (font is double)
            {
                return (double)font;
            }

            throw new Exception($"The font size is not valid.");
        }

        public override Style GetStyle(object resourceKey)
        {
            var style = Application.Current.FindResource(resourceKey);
            if (style is Style)
            {
                return (Style)style;
            }

            throw new Exception($"The style is not valid.");
        }
    }
}
