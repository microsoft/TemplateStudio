// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Media;

namespace Microsoft.Templates.UI.Services
{
    public partial class UIStylesService
    {
        // Button Colors
        public static readonly DependencyProperty ButtonProperty = DependencyProperty.Register("Button", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush Button
        {
            get { return (Brush)GetValue(ButtonProperty); }
            set { SetValue(ButtonProperty, value); }
        }

        public static readonly DependencyProperty ButtonTextProperty = DependencyProperty.Register("ButtonText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ButtonText
        {
            get { return (Brush)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }

        public static readonly DependencyProperty ButtonBorderProperty = DependencyProperty.Register("ButtonBorder", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ButtonBorder
        {
            get { return (Brush)GetValue(ButtonBorderProperty); }
            set { SetValue(ButtonBorderProperty, value); }
        }

        // Button Default Colors
        public static readonly DependencyProperty ButtonDefaultProperty = DependencyProperty.Register("ButtonDefault", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ButtonDefault
        {
            get { return (Brush)GetValue(ButtonDefaultProperty); }
            set { SetValue(ButtonDefaultProperty, value); }
        }

        public static readonly DependencyProperty ButtonDefaultTextProperty = DependencyProperty.Register("ButtonDefaultText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ButtonDefaultText
        {
            get { return (Brush)GetValue(ButtonDefaultTextProperty); }
            set { SetValue(ButtonDefaultTextProperty, value); }
        }

        public static readonly DependencyProperty ButtonBorderDefaultProperty = DependencyProperty.Register("ButtonBorderDefault", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ButtonBorderDefault
        {
            get { return (Brush)GetValue(ButtonBorderDefaultProperty); }
            set { SetValue(ButtonBorderDefaultProperty, value); }
        }

        public static readonly DependencyProperty ButtonDisabledProperty = DependencyProperty.Register("ButtonDisabled", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        // Button Disabled Colors
        public Brush ButtonDisabled
        {
            get { return (Brush)GetValue(ButtonDisabledProperty); }
            set { SetValue(ButtonDisabledProperty, value); }
        }

        public static readonly DependencyProperty ButtonDisabledTextProperty = DependencyProperty.Register("ButtonDisabledText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ButtonDisabledText
        {
            get { return (Brush)GetValue(ButtonDisabledTextProperty); }
            set { SetValue(ButtonDisabledTextProperty, value); }
        }

        public static readonly DependencyProperty ButtonBorderDisabledProperty = DependencyProperty.Register("ButtonBorderDisabled", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ButtonBorderDisabled
        {
            get { return (Brush)GetValue(ButtonBorderDisabledProperty); }
            set { SetValue(ButtonBorderDisabledProperty, value); }
        }

        // Button Focused Colors
        public static readonly DependencyProperty ButtonFocusedProperty = DependencyProperty.Register("ButtonFocused", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ButtonFocused
        {
            get { return (Brush)GetValue(ButtonFocusedProperty); }
            set { SetValue(ButtonFocusedProperty, value); }
        }

        public static readonly DependencyProperty ButtonFocusedTextProperty = DependencyProperty.Register("ButtonFocusedText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ButtonFocusedText
        {
            get { return (Brush)GetValue(ButtonFocusedTextProperty); }
            set { SetValue(ButtonFocusedTextProperty, value); }
        }

        public static readonly DependencyProperty ButtonBorderFocusedProperty = DependencyProperty.Register("ButtonBorderFocused", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ButtonBorderFocused
        {
            get { return (Brush)GetValue(ButtonBorderFocusedProperty); }
            set { SetValue(ButtonBorderFocusedProperty, value); }
        }

        // Button Hover Colors
        public static readonly DependencyProperty ButtonHoverProperty = DependencyProperty.Register("ButtonHover", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ButtonHover
        {
            get { return (Brush)GetValue(ButtonHoverProperty); }
            set { SetValue(ButtonHoverProperty, value); }
        }

        public static readonly DependencyProperty ButtonHoverTextProperty = DependencyProperty.Register("ButtonHoverText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ButtonHoverText
        {
            get { return (Brush)GetValue(ButtonHoverTextProperty); }
            set { SetValue(ButtonHoverTextProperty, value); }
        }

        public static readonly DependencyProperty ButtonBorderHoverProperty = DependencyProperty.Register("ButtonBorderHover", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ButtonBorderHover
        {
            get { return (Brush)GetValue(ButtonBorderHoverProperty); }
            set { SetValue(ButtonBorderHoverProperty, value); }
        }

        // Button Pressed Colors
        public static readonly DependencyProperty ButtonPressedProperty = DependencyProperty.Register("ButtonPressed", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ButtonPressed
        {
            get { return (Brush)GetValue(ButtonPressedProperty); }
            set { SetValue(ButtonPressedProperty, value); }
        }

        public static readonly DependencyProperty ButtonPressedTextProperty = DependencyProperty.Register("ButtonPressedText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ButtonPressedText
        {
            get { return (Brush)GetValue(ButtonPressedTextProperty); }
            set { SetValue(ButtonPressedTextProperty, value); }
        }

        public static readonly DependencyProperty ButtonBorderPressedProperty = DependencyProperty.Register("ButtonBorderPressed", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ButtonBorderPressed
        {
            get { return (Brush)GetValue(ButtonBorderPressedProperty); }
            set { SetValue(ButtonBorderPressedProperty, value); }
        }

        // ComboBox Colors
        public static readonly DependencyProperty ComboBoxBackgroundProperty = DependencyProperty.Register("ComboBoxBackground", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxBackground
        {
            get { return (Brush)GetValue(ComboBoxBackgroundProperty); }
            set { SetValue(ComboBoxBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxBackgroundDisabledProperty = DependencyProperty.Register("ComboBoxBackgroundDisabled", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxBackgroundDisabled
        {
            get { return (Brush)GetValue(ComboBoxBackgroundDisabledProperty); }
            set { SetValue(ComboBoxBackgroundDisabledProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxBackgroundFocusedProperty = DependencyProperty.Register("ComboBoxBackgroundFocused", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxBackgroundFocused
        {
            get { return (Brush)GetValue(ComboBoxBackgroundFocusedProperty); }
            set { SetValue(ComboBoxBackgroundFocusedProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxBackgroundHoverProperty = DependencyProperty.Register("ComboBoxBackgroundHover", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxBackgroundHover
        {
            get { return (Brush)GetValue(ComboBoxBackgroundHoverProperty); }
            set { SetValue(ComboBoxBackgroundHoverProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxBackgroundPressedProperty = DependencyProperty.Register("ComboBoxBackgroundPressed", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxBackgroundPressed
        {
            get { return (Brush)GetValue(ComboBoxBackgroundPressedProperty); }
            set { SetValue(ComboBoxBackgroundPressedProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxBorderProperty = DependencyProperty.Register("ComboBoxBorder", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxBorder
        {
            get { return (Brush)GetValue(ComboBoxBorderProperty); }
            set { SetValue(ComboBoxBorderProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxBorderDisabledProperty = DependencyProperty.Register("ComboBoxBorderDisabled", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxBorderDisabled
        {
            get { return (Brush)GetValue(ComboBoxBorderDisabledProperty); }
            set { SetValue(ComboBoxBorderDisabledProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxBorderFocusedProperty = DependencyProperty.Register("ComboBoxBorderFocused", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxBorderFocused
        {
            get { return (Brush)GetValue(ComboBoxBorderFocusedProperty); }
            set { SetValue(ComboBoxBorderFocusedProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxBorderHoverProperty = DependencyProperty.Register("ComboBoxBorderHover", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxBorderHover
        {
            get { return (Brush)GetValue(ComboBoxBorderHoverProperty); }
            set { SetValue(ComboBoxBorderHoverProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxBorderPressedProperty = DependencyProperty.Register("ComboBoxBorderPressed", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxBorderPressed
        {
            get { return (Brush)GetValue(ComboBoxBorderPressedProperty); }
            set { SetValue(ComboBoxBorderPressedProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxGlyphProperty = DependencyProperty.Register("ComboBoxGlyph", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxGlyph
        {
            get { return (Brush)GetValue(ComboBoxGlyphProperty); }
            set { SetValue(ComboBoxGlyphProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxGlyphBackgroundProperty = DependencyProperty.Register("ComboBoxGlyphBackground", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxGlyphBackground
        {
            get { return (Brush)GetValue(ComboBoxGlyphBackgroundProperty); }
            set { SetValue(ComboBoxGlyphBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxGlyphBackgroundDisabledProperty = DependencyProperty.Register("ComboBoxGlyphBackgroundDisabled", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxGlyphBackgroundDisabled
        {
            get { return (Brush)GetValue(ComboBoxGlyphBackgroundDisabledProperty); }
            set { SetValue(ComboBoxGlyphBackgroundDisabledProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxGlyphBackgroundFocusedProperty = DependencyProperty.Register("ComboBoxGlyphBackgroundFocused", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxGlyphBackgroundFocused
        {
            get { return (Brush)GetValue(ComboBoxGlyphBackgroundFocusedProperty); }
            set { SetValue(ComboBoxGlyphBackgroundFocusedProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxGlyphBackgroundHoverProperty = DependencyProperty.Register("ComboBoxGlyphBackgroundHover", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxGlyphBackgroundHover
        {
            get { return (Brush)GetValue(ComboBoxGlyphBackgroundHoverProperty); }
            set { SetValue(ComboBoxGlyphBackgroundHoverProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxGlyphBackgroundPressedProperty = DependencyProperty.Register("ComboBoxGlyphBackgroundPressed", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxGlyphBackgroundPressed
        {
            get { return (Brush)GetValue(ComboBoxGlyphBackgroundPressedProperty); }
            set { SetValue(ComboBoxGlyphBackgroundPressedProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxGlyphDisabledProperty = DependencyProperty.Register("ComboBoxGlyphDisabled", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxGlyphDisabled
        {
            get { return (Brush)GetValue(ComboBoxGlyphDisabledProperty); }
            set { SetValue(ComboBoxGlyphDisabledProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxGlyphFocusedProperty = DependencyProperty.Register("ComboBoxGlyphFocused", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxGlyphFocused
        {
            get { return (Brush)GetValue(ComboBoxGlyphFocusedProperty); }
            set { SetValue(ComboBoxGlyphFocusedProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxGlyphHoverProperty = DependencyProperty.Register("ComboBoxGlyphHover", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxGlyphHover
        {
            get { return (Brush)GetValue(ComboBoxGlyphHoverProperty); }
            set { SetValue(ComboBoxGlyphHoverProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxGlyphPressedProperty = DependencyProperty.Register("ComboBoxGlyphPressed", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxGlyphPressed
        {
            get { return (Brush)GetValue(ComboBoxGlyphPressedProperty); }
            set { SetValue(ComboBoxGlyphPressedProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxListBackgroundProperty = DependencyProperty.Register("ComboBoxListBackground", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxListBackground
        {
            get { return (Brush)GetValue(ComboBoxListBackgroundProperty); }
            set { SetValue(ComboBoxListBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxListBackgroundShadowProperty = DependencyProperty.Register("ComboBoxListBackgroundShadow", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxListBackgroundShadow
        {
            get { return (Brush)GetValue(ComboBoxListBackgroundShadowProperty); }
            set { SetValue(ComboBoxListBackgroundShadowProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxListBorderProperty = DependencyProperty.Register("ComboBoxListBorder", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxListBorder
        {
            get { return (Brush)GetValue(ComboBoxListBorderProperty); }
            set { SetValue(ComboBoxListBorderProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxListItemBackgroundHoverProperty = DependencyProperty.Register("ComboBoxListItemBackgroundHover", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxListItemBackgroundHover
        {
            get { return (Brush)GetValue(ComboBoxListItemBackgroundHoverProperty); }
            set { SetValue(ComboBoxListItemBackgroundHoverProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxListItemBorderHoverProperty = DependencyProperty.Register("ComboBoxListItemBorderHover", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxListItemBorderHover
        {
            get { return (Brush)GetValue(ComboBoxListItemBorderHoverProperty); }
            set { SetValue(ComboBoxListItemBorderHoverProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxListItemTextProperty = DependencyProperty.Register("ComboBoxListItemText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxListItemText
        {
            get { return (Brush)GetValue(ComboBoxListItemTextProperty); }
            set { SetValue(ComboBoxListItemTextProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxListItemTextHoverProperty = DependencyProperty.Register("ComboBoxListItemTextHover", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxListItemTextHover
        {
            get { return (Brush)GetValue(ComboBoxListItemTextHoverProperty); }
            set { SetValue(ComboBoxListItemTextHoverProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxSelectionProperty = DependencyProperty.Register("ComboBoxSelection", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxSelection
        {
            get { return (Brush)GetValue(ComboBoxSelectionProperty); }
            set { SetValue(ComboBoxSelectionProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxSeparatorProperty = DependencyProperty.Register("ComboBoxSeparator", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxSeparator
        {
            get { return (Brush)GetValue(ComboBoxSeparatorProperty); }
            set { SetValue(ComboBoxSeparatorProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxSeparatorDisabledProperty = DependencyProperty.Register("ComboBoxSeparatorDisabled", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxSeparatorDisabled
        {
            get { return (Brush)GetValue(ComboBoxSeparatorDisabledProperty); }
            set { SetValue(ComboBoxSeparatorDisabledProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxSeparatorFocusedProperty = DependencyProperty.Register("ComboBoxSeparatorFocused", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxSeparatorFocused
        {
            get { return (Brush)GetValue(ComboBoxSeparatorFocusedProperty); }
            set { SetValue(ComboBoxSeparatorFocusedProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxSeparatorHoverProperty = DependencyProperty.Register("ComboBoxSeparatorHover", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxSeparatorHover
        {
            get { return (Brush)GetValue(ComboBoxSeparatorHoverProperty); }
            set { SetValue(ComboBoxSeparatorHoverProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxSeparatorPressedProperty = DependencyProperty.Register("ComboBoxSeparatorPressed", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxSeparatorPressed
        {
            get { return (Brush)GetValue(ComboBoxSeparatorPressedProperty); }
            set { SetValue(ComboBoxSeparatorPressedProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxTextProperty = DependencyProperty.Register("ComboBoxText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxText
        {
            get { return (Brush)GetValue(ComboBoxTextProperty); }
            set { SetValue(ComboBoxTextProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxTextDisabledProperty = DependencyProperty.Register("ComboBoxTextDisabled", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxTextDisabled
        {
            get { return (Brush)GetValue(ComboBoxTextDisabledProperty); }
            set { SetValue(ComboBoxTextDisabledProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxTextFocusedProperty = DependencyProperty.Register("ComboBoxTextFocused", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxTextFocused
        {
            get { return (Brush)GetValue(ComboBoxTextFocusedProperty); }
            set { SetValue(ComboBoxTextFocusedProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxTextHoverProperty = DependencyProperty.Register("ComboBoxTextHover", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxTextHover
        {
            get { return (Brush)GetValue(ComboBoxTextHoverProperty); }
            set { SetValue(ComboBoxTextHoverProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxTextInputSelectionProperty = DependencyProperty.Register("ComboBoxTextInputSelection", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxTextInputSelection
        {
            get { return (Brush)GetValue(ComboBoxTextInputSelectionProperty); }
            set { SetValue(ComboBoxTextInputSelectionProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxTextPressedProperty = DependencyProperty.Register("ComboBoxTextPressed", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ComboBoxTextPressed
        {
            get { return (Brush)GetValue(ComboBoxTextPressedProperty); }
            set { SetValue(ComboBoxTextPressedProperty, value); }
        }

        // TextBox Colors
        public static readonly DependencyProperty TextBoxBackgroundProperty = DependencyProperty.Register("TextBoxBackground", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush TextBoxBackground
        {
            get { return (Brush)GetValue(TextBoxBackgroundProperty); }
            set { SetValue(TextBoxBackgroundProperty, value); }
        }

        public static readonly DependencyProperty TextBoxTextProperty = DependencyProperty.Register("TextBoxText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush TextBoxText
        {
            get { return (Brush)GetValue(TextBoxTextProperty); }
            set { SetValue(TextBoxTextProperty, value); }
        }

        public static readonly DependencyProperty TextBoxBorderProperty = DependencyProperty.Register("TextBoxBorder", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush TextBoxBorder
        {
            get { return (Brush)GetValue(TextBoxBorderProperty); }
            set { SetValue(TextBoxBorderProperty, value); }
        }

        // TextBox Disabled Colors
        public static readonly DependencyProperty TextBoxBackgroundDisabledProperty = DependencyProperty.Register("TextBoxBackgroundDisabled", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush TextBoxBackgroundDisabled
        {
            get { return (Brush)GetValue(TextBoxBackgroundDisabledProperty); }
            set { SetValue(TextBoxBackgroundDisabledProperty, value); }
        }

        public static readonly DependencyProperty TextBoxBorderDisabledProperty = DependencyProperty.Register("TextBoxBorderDisabled", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush TextBoxBorderDisabled
        {
            get { return (Brush)GetValue(TextBoxBorderDisabledProperty); }
            set { SetValue(TextBoxBorderDisabledProperty, value); }
        }

        public static readonly DependencyProperty TextBoxTextDisabledProperty = DependencyProperty.Register("TextBoxTextDisabled", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush TextBoxTextDisabled
        {
            get { return (Brush)GetValue(TextBoxTextDisabledProperty); }
            set { SetValue(TextBoxTextDisabledProperty, value); }
        }

        // TextBox Focused Colors
        public static readonly DependencyProperty TextBoxBackgroundFocusedProperty = DependencyProperty.Register("TextBoxBackgroundFocused", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush TextBoxBackgroundFocused
        {
            get { return (Brush)GetValue(TextBoxBackgroundFocusedProperty); }
            set { SetValue(TextBoxBackgroundFocusedProperty, value); }
        }

        public static readonly DependencyProperty TextBoxBorderFocusedProperty = DependencyProperty.Register("TextBoxBorderFocused", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush TextBoxBorderFocused
        {
            get { return (Brush)GetValue(TextBoxBorderFocusedProperty); }
            set { SetValue(TextBoxBorderFocusedProperty, value); }
        }

        public static readonly DependencyProperty TextBoxTextFocusedProperty = DependencyProperty.Register("TextBoxTextFocused", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush TextBoxTextFocused
        {
            get { return (Brush)GetValue(TextBoxTextFocusedProperty); }
            set { SetValue(TextBoxTextFocusedProperty, value); }
        }

        public static readonly DependencyProperty CheckBoxGlyphProperty = DependencyProperty.Register("CheckBoxGlyph", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CheckBoxGlyph
        {
            get { return (Brush)GetValue(CheckBoxGlyphProperty); }
            set { SetValue(CheckBoxGlyphProperty, value); }
        }

        public static readonly DependencyProperty CheckBoxGlyphDisabledProperty = DependencyProperty.Register("CheckBoxGlyphDisabled", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CheckBoxGlyphDisabled
        {
            get { return (Brush)GetValue(CheckBoxGlyphDisabledProperty); }
            set { SetValue(CheckBoxGlyphDisabledProperty, value); }
        }

        public static readonly DependencyProperty CheckBoxGlyphFocusedProperty = DependencyProperty.Register("CheckBoxGlyphFocused", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CheckBoxGlyphFocused
        {
            get { return (Brush)GetValue(CheckBoxGlyphFocusedProperty); }
            set { SetValue(CheckBoxGlyphFocusedProperty, value); }
        }

        public static readonly DependencyProperty CheckBoxGlyphHoverProperty = DependencyProperty.Register("CheckBoxGlyphHover", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CheckBoxGlyphHover
        {
            get { return (Brush)GetValue(CheckBoxGlyphHoverProperty); }
            set { SetValue(CheckBoxGlyphHoverProperty, value); }
        }

        public static readonly DependencyProperty CheckBoxGlyphPressedProperty = DependencyProperty.Register("CheckBoxGlyphPressed", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CheckBoxGlyphPressed
        {
            get { return (Brush)GetValue(CheckBoxGlyphPressedProperty); }
            set { SetValue(CheckBoxGlyphPressedProperty, value); }
        }

        public static readonly DependencyProperty CheckBoxBackgroundProperty = DependencyProperty.Register("CheckBoxBackground", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CheckBoxBackground
        {
            get { return (Brush)GetValue(CheckBoxBackgroundProperty); }
            set { SetValue(CheckBoxBackgroundProperty, value); }
        }

        public static readonly DependencyProperty CheckBoxBackgroundDisabledProperty = DependencyProperty.Register("CheckBoxBackgroundDisabled", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CheckBoxBackgroundDisabled
        {
            get { return (Brush)GetValue(CheckBoxBackgroundDisabledProperty); }
            set { SetValue(CheckBoxBackgroundDisabledProperty, value); }
        }

        public static readonly DependencyProperty CheckBoxBackgroundFocusedProperty = DependencyProperty.Register("CheckBoxBackgroundFocused", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CheckBoxBackgroundFocused
        {
            get { return (Brush)GetValue(CheckBoxBackgroundFocusedProperty); }
            set { SetValue(CheckBoxBackgroundFocusedProperty, value); }
        }

        public static readonly DependencyProperty CheckBoxBackgroundHoverProperty = DependencyProperty.Register("CheckBoxBackgroundHover", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CheckBoxBackgroundHover
        {
            get { return (Brush)GetValue(CheckBoxBackgroundHoverProperty); }
            set { SetValue(CheckBoxBackgroundHoverProperty, value); }
        }

        public static readonly DependencyProperty CheckBoxBackgroundPressedProperty = DependencyProperty.Register("CheckBoxBackgroundPressed", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CheckBoxBackgroundPressed
        {
            get { return (Brush)GetValue(CheckBoxBackgroundPressedProperty); }
            set { SetValue(CheckBoxBackgroundPressedProperty, value); }
        }

        public static readonly DependencyProperty CheckBoxBorderProperty = DependencyProperty.Register("CheckBoxBorder", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CheckBoxBorder
        {
            get { return (Brush)GetValue(CheckBoxBorderProperty); }
            set { SetValue(CheckBoxBorderProperty, value); }
        }

        public static readonly DependencyProperty CheckBoxBorderDisabledProperty = DependencyProperty.Register("CheckBoxBorderDisabled", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CheckBoxBorderDisabled
        {
            get { return (Brush)GetValue(CheckBoxBorderDisabledProperty); }
            set { SetValue(CheckBoxBorderDisabledProperty, value); }
        }

        public static readonly DependencyProperty CheckBoxBorderFocusedProperty = DependencyProperty.Register("CheckBoxBorderFocused", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CheckBoxBorderFocused
        {
            get { return (Brush)GetValue(CheckBoxBorderFocusedProperty); }
            set { SetValue(CheckBoxBorderFocusedProperty, value); }
        }

        public static readonly DependencyProperty CheckBoxBorderHoverProperty = DependencyProperty.Register("CheckBoxBorderHover", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CheckBoxBorderHover
        {
            get { return (Brush)GetValue(CheckBoxBorderHoverProperty); }
            set { SetValue(CheckBoxBorderHoverProperty, value); }
        }

        public static readonly DependencyProperty CheckBoxBorderPressedProperty = DependencyProperty.Register("CheckBoxBorderPressed", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CheckBoxBorderPressed
        {
            get { return (Brush)GetValue(CheckBoxBorderPressedProperty); }
            set { SetValue(CheckBoxBorderPressedProperty, value); }
        }

        public static readonly DependencyProperty CheckBoxTextProperty = DependencyProperty.Register("CheckBoxText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CheckBoxText
        {
            get { return (Brush)GetValue(CheckBoxTextProperty); }
            set { SetValue(CheckBoxTextProperty, value); }
        }

        public static readonly DependencyProperty CheckBoxTextDisabledProperty = DependencyProperty.Register("CheckBoxTextDisabled", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CheckBoxTextDisabled
        {
            get { return (Brush)GetValue(CheckBoxTextDisabledProperty); }
            set { SetValue(CheckBoxTextDisabledProperty, value); }
        }

        public static readonly DependencyProperty CheckBoxTextFocusedProperty = DependencyProperty.Register("CheckBoxTextFocused", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CheckBoxTextFocused
        {
            get { return (Brush)GetValue(CheckBoxTextFocusedProperty); }
            set { SetValue(CheckBoxTextFocusedProperty, value); }
        }

        public static readonly DependencyProperty CheckBoxTextHoverProperty = DependencyProperty.Register("CheckBoxTextHover", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CheckBoxTextHover
        {
            get { return (Brush)GetValue(CheckBoxTextHoverProperty); }
            set { SetValue(CheckBoxTextHoverProperty, value); }
        }

        public static readonly DependencyProperty CheckBoxTextPressedProperty = DependencyProperty.Register("CheckBoxTextPressed", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CheckBoxTextPressed
        {
            get { return (Brush)GetValue(CheckBoxTextPressedProperty); }
            set { SetValue(CheckBoxTextPressedProperty, value); }
        }
    }
}
