// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

using Microsoft.Templates.UI.V2ViewModels.Common;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.Templates.UI.V2ViewModels.NewProject
{
    public class MainViewModel : BaseMainViewModel
    {
        public ObservableCollection<ColorItem> ColorItems { get;  } = new ObservableCollection<ColorItem>();

        public ObservableCollection<ColorItem> ColorItems_Tests { get;  } = new ObservableCollection<ColorItem>();

        public void LoadData()
        {
            ColorItems_Tests.Add(new ColorItem()
            {
                Name = ThemedDialogColors.WindowPanelColorKey.Name,
                FullName = ThemedDialogColors.WindowPanelColorKey.ToString(),
                Background = GetColor(ThemedDialogColors.WindowPanelColorKey),
                Code = GetHexaColor(ThemedDialogColors.WindowPanelColorKey)
            });

            ColorItems_Tests.Add(new ColorItem()
            {
                Name = ThemedDialogColors.WindowBorderColorKey.Name,
                FullName = ThemedDialogColors.WindowBorderColorKey.ToString(),
                Background = GetColor(ThemedDialogColors.WindowBorderColorKey),
                Code = GetHexaColor(ThemedDialogColors.WindowBorderColorKey)
            });

            ColorItems_Tests.Add(new ColorItem()
            {
                Name = ThemedDialogColors.HeaderTextColorKey.Name,
                FullName = ThemedDialogColors.HeaderTextColorKey.ToString(),
                Background = GetColor(ThemedDialogColors.HeaderTextColorKey),
                Code = GetHexaColor(ThemedDialogColors.HeaderTextColorKey)
            });

            ColorItems_Tests.Add(new ColorItem()
            {
                Name = ThemedDialogColors.HyperlinkColorKey.Name,
                FullName = ThemedDialogColors.HyperlinkColorKey.ToString(),
                Background = GetColor(ThemedDialogColors.HyperlinkColorKey),
                Code = GetHexaColor(ThemedDialogColors.HyperlinkColorKey)
            });

            ColorItems_Tests.Add(new ColorItem()
            {
                Name = ThemedDialogColors.HyperlinkHoverColorKey.Name,
                FullName = ThemedDialogColors.HyperlinkHoverColorKey.ToString(),
                Background = GetColor(ThemedDialogColors.HyperlinkHoverColorKey),
                Code = GetHexaColor(ThemedDialogColors.HyperlinkHoverColorKey)
            });

            ColorItems_Tests.Add(new ColorItem()
            {
                Name = ThemedDialogColors.HyperlinkPressedColorKey.Name,
                FullName = ThemedDialogColors.HyperlinkPressedColorKey.ToString(),
                Background = GetColor(ThemedDialogColors.HyperlinkPressedColorKey),
                Code = GetHexaColor(ThemedDialogColors.HyperlinkPressedColorKey)
            });

            ColorItems_Tests.Add(new ColorItem()
            {
                Name = ThemedDialogColors.HyperlinkDisabledColorKey.Name,
                FullName = ThemedDialogColors.HyperlinkDisabledColorKey.ToString(),
                Background = GetColor(ThemedDialogColors.HyperlinkDisabledColorKey),
                Code = GetHexaColor(ThemedDialogColors.HyperlinkDisabledColorKey)
            });

            ColorItems_Tests.Add(new ColorItem()
            {
                Name = ThemedDialogColors.SelectedItemActiveColorKey.Name,
                FullName = ThemedDialogColors.SelectedItemActiveColorKey.ToString(),
                Background = GetColor(ThemedDialogColors.SelectedItemActiveColorKey),
                Code = GetHexaColor(ThemedDialogColors.SelectedItemActiveColorKey)
            });

            ColorItems_Tests.Add(new ColorItem()
            {
                Name = ThemedDialogColors.SelectedItemInactiveColorKey.Name,
                FullName = ThemedDialogColors.SelectedItemInactiveColorKey.ToString(),
                Background = GetColor(ThemedDialogColors.SelectedItemInactiveColorKey),
                Code = GetHexaColor(ThemedDialogColors.SelectedItemInactiveColorKey)
            });

            ColorItems_Tests.Add(new ColorItem()
            {
                Name = ThemedDialogColors.ListItemMouseOverColorKey.Name,
                FullName = ThemedDialogColors.ListItemMouseOverColorKey.ToString(),
                Background = GetColor(ThemedDialogColors.ListItemMouseOverColorKey),
                Code = GetHexaColor(ThemedDialogColors.ListItemMouseOverColorKey)
            });

            ColorItems_Tests.Add(new ColorItem()
            {
                Name = ThemedDialogColors.ListItemDisabledTextColorKey.Name,
                FullName = ThemedDialogColors.ListItemDisabledTextColorKey.ToString(),
                Background = GetColor(ThemedDialogColors.ListItemDisabledTextColorKey),
                Code = GetHexaColor(ThemedDialogColors.ListItemDisabledTextColorKey)
            });

            ColorItems_Tests.Add(new ColorItem()
            {
                Name = ThemedDialogColors.GridHeadingBackgroundColorKey.Name,
                FullName = ThemedDialogColors.GridHeadingBackgroundColorKey.ToString(),
                Background = GetColor(ThemedDialogColors.GridHeadingBackgroundColorKey),
                Code = GetHexaColor(ThemedDialogColors.GridHeadingBackgroundColorKey)
            });

            ColorItems_Tests.Add(new ColorItem()
            {
                Name = ThemedDialogColors.GridHeadingHoverBackgroundColorKey.Name,
                FullName = ThemedDialogColors.GridHeadingHoverBackgroundColorKey.ToString(),
                Background = GetColor(ThemedDialogColors.GridHeadingHoverBackgroundColorKey),
                Code = GetHexaColor(ThemedDialogColors.GridHeadingHoverBackgroundColorKey)
            });

            ColorItems_Tests.Add(new ColorItem()
            {
                Name = ThemedDialogColors.GridHeadingTextColorKey.Name,
                FullName = ThemedDialogColors.GridHeadingTextColorKey.ToString(),
                Background = GetColor(ThemedDialogColors.GridHeadingTextColorKey),
                Code = GetHexaColor(ThemedDialogColors.GridHeadingTextColorKey)
            });

            ColorItems_Tests.Add(new ColorItem()
            {
                Name = ThemedDialogColors.GridHeadingHoverTextColorKey.Name,
                FullName = ThemedDialogColors.GridHeadingHoverTextColorKey.ToString(),
                Background = GetColor(ThemedDialogColors.GridHeadingHoverTextColorKey),
                Code = GetHexaColor(ThemedDialogColors.GridHeadingHoverTextColorKey)
            });

            ColorItems_Tests.Add(new ColorItem()
            {
                Name = ThemedDialogColors.GridLineColorKey.Name,
                FullName = ThemedDialogColors.GridLineColorKey.ToString(),
                Background = GetColor(ThemedDialogColors.GridLineColorKey),
                Code = GetHexaColor(ThemedDialogColors.GridLineColorKey)
            });

            ColorItems_Tests.Add(new ColorItem()
            {
                Name = ThemedDialogColors.SectionDividerColorKey.Name,
                FullName = ThemedDialogColors.SectionDividerColorKey.ToString(),
                Background = GetColor(ThemedDialogColors.SectionDividerColorKey),
                Code = GetHexaColor(ThemedDialogColors.SectionDividerColorKey)
            });

            ColorItems_Tests.Add(new ColorItem()
            {
                Name = ThemedDialogColors.WindowButtonColorKey.Name,
                FullName = ThemedDialogColors.WindowButtonColorKey.ToString(),
                Background = GetColor(ThemedDialogColors.WindowButtonColorKey),
                Code = GetHexaColor(ThemedDialogColors.WindowButtonColorKey)
            });

            ColorItems_Tests.Add(new ColorItem()
            {
                Name = ThemedDialogColors.WindowButtonHoverColorKey.Name,
                FullName = ThemedDialogColors.WindowButtonHoverColorKey.ToString(),
                Background = GetColor(ThemedDialogColors.WindowButtonHoverColorKey),
                Code = GetHexaColor(ThemedDialogColors.WindowButtonHoverColorKey)
            });

            ColorItems_Tests.Add(new ColorItem()
            {
                Name = ThemedDialogColors.WindowButtonDownColorKey.Name,
                FullName = ThemedDialogColors.WindowButtonDownColorKey.ToString(),
                Background = GetColor(ThemedDialogColors.WindowButtonDownColorKey),
                Code = GetHexaColor(ThemedDialogColors.WindowButtonDownColorKey)
            });

            ColorItems_Tests.Add(new ColorItem()
            {
                Name = ThemedDialogColors.WindowButtonBorderColorKey.Name,
                FullName = ThemedDialogColors.WindowButtonBorderColorKey.ToString(),
                Background = GetColor(ThemedDialogColors.WindowButtonBorderColorKey),
                Code = GetHexaColor(ThemedDialogColors.WindowButtonBorderColorKey)
            });

            ColorItems_Tests.Add(new ColorItem()
            {
                Name = ThemedDialogColors.WindowButtonHoverBorderColorKey.Name,
                FullName = ThemedDialogColors.WindowButtonHoverBorderColorKey.ToString(),
                Background = GetColor(ThemedDialogColors.WindowButtonHoverBorderColorKey),
                Code = GetHexaColor(ThemedDialogColors.WindowButtonHoverBorderColorKey)
            });

            ColorItems_Tests.Add(new ColorItem()
            {
                Name = ThemedDialogColors.WindowButtonDownBorderColorKey.Name,
                FullName = ThemedDialogColors.WindowButtonDownBorderColorKey.ToString(),
                Background = GetColor(ThemedDialogColors.WindowButtonDownBorderColorKey),
                Code = GetHexaColor(ThemedDialogColors.WindowButtonDownBorderColorKey)
            });

            ColorItems_Tests.Add(new ColorItem()
            {
                Name = ThemedDialogColors.WindowButtonGlyphColorKey.Name,
                FullName = ThemedDialogColors.WindowButtonGlyphColorKey.ToString(),
                Background = GetColor(ThemedDialogColors.WindowButtonGlyphColorKey),
                Code = GetHexaColor(ThemedDialogColors.WindowButtonGlyphColorKey)
            });

            ColorItems_Tests.Add(new ColorItem()
            {
                Name = ThemedDialogColors.WindowButtonHoverGlyphColorKey.Name,
                FullName = ThemedDialogColors.WindowButtonHoverGlyphColorKey.ToString(),
                Background = GetColor(ThemedDialogColors.WindowButtonHoverGlyphColorKey),
                Code = GetHexaColor(ThemedDialogColors.WindowButtonHoverGlyphColorKey)
            });

            ColorItems_Tests.Add(new ColorItem()
            {
                Name = ThemedDialogColors.WindowButtonDownGlyphColorKey.Name,
                FullName = ThemedDialogColors.WindowButtonDownGlyphColorKey.ToString(),
                Background = GetColor(ThemedDialogColors.WindowButtonDownGlyphColorKey),
                Code = GetHexaColor(ThemedDialogColors.WindowButtonDownGlyphColorKey)
            });

            ColorItems_Tests.Add(new ColorItem()
            {
                Name = ThemedDialogColors.WizardFooterColorKey.Name,
                FullName = ThemedDialogColors.WizardFooterColorKey.ToString(),
                Background = GetColor(ThemedDialogColors.WizardFooterColorKey),
                Code = GetHexaColor(ThemedDialogColors.WizardFooterColorKey)
            });
        }

        private Brush GetColor(ThemeResourceKey themeResourceKey)
        {
            var themeColor = VSColorTheme.GetThemedColor(themeResourceKey);
            return new SolidColorBrush(new Color()
            {
                A = themeColor.A,
                R = themeColor.R,
                G = themeColor.G,
                B = themeColor.B
            });
        }

        private string GetHexaColor(ThemeResourceKey themeResourceKey)
        {
            var themeColor = VSColorTheme.GetThemedColor(themeResourceKey);
            return $"{themeColor.A.ToString("X2")} {themeColor.R.ToString("X2")}{themeColor.G.ToString("X2")}{themeColor.B.ToString("X2")}";
        }
    }
}
