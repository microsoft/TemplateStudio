// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Media;

namespace Microsoft.Templates.UI.Services
{
    public partial class UIStylesService
    {
        public static readonly DependencyProperty IBButtonProperty = DependencyProperty.Register("IBButton", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush IBButton
        {
            get { return (Brush)GetValue(IBButtonProperty); }
            set { SetValue(IBButtonProperty, value); }
        }

        public static readonly DependencyProperty IBButtonBorderProperty = DependencyProperty.Register("IBButtonBorder", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush IBButtonBorder
        {
            get { return (Brush)GetValue(IBButtonBorderProperty); }
            set { SetValue(IBButtonBorderProperty, value); }
        }

        public static readonly DependencyProperty IBButtonDisabledProperty = DependencyProperty.Register("IBButtonDisabled", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush IBButtonDisabled
        {
            get { return (Brush)GetValue(IBButtonDisabledProperty); }
            set { SetValue(IBButtonDisabledProperty, value); }
        }

        public static readonly DependencyProperty IBButtonDisabledBorderProperty = DependencyProperty.Register("IBButtonDisabledBorder", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush IBButtonDisabledBorder
        {
            get { return (Brush)GetValue(IBButtonDisabledBorderProperty); }
            set { SetValue(IBButtonDisabledBorderProperty, value); }
        }

        public static readonly DependencyProperty IBButtonFocusProperty = DependencyProperty.Register("IBButtonFocus", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush IBButtonFocus
        {
            get { return (Brush)GetValue(IBButtonFocusProperty); }
            set { SetValue(IBButtonFocusProperty, value); }
        }

        public static readonly DependencyProperty IBButtonFocusBorderProperty = DependencyProperty.Register("IBButtonFocusBorder", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush IBButtonFocusBorder
        {
            get { return (Brush)GetValue(IBButtonFocusBorderProperty); }
            set { SetValue(IBButtonFocusBorderProperty, value); }
        }

        public static readonly DependencyProperty IBButtonMouseDownProperty = DependencyProperty.Register("IBButtonMouseDown", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush IBButtonMouseDown
        {
            get { return (Brush)GetValue(IBButtonMouseDownProperty); }
            set { SetValue(IBButtonMouseDownProperty, value); }
        }

        public static readonly DependencyProperty IBButtonMouseDownBorderProperty = DependencyProperty.Register("IBButtonMouseDownBorder", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush IBButtonMouseDownBorder
        {
            get { return (Brush)GetValue(IBButtonMouseDownBorderProperty); }
            set { SetValue(IBButtonMouseDownBorderProperty, value); }
        }

        public static readonly DependencyProperty IBButtonMouseOverProperty = DependencyProperty.Register("IBButtonMouseOver", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush IBButtonMouseOver
        {
            get { return (Brush)GetValue(IBButtonMouseOverProperty); }
            set { SetValue(IBButtonMouseOverProperty, value); }
        }

        public static readonly DependencyProperty IBButtonMouseOverBorderProperty = DependencyProperty.Register("IBButtonMouseOverBorder", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush IBButtonMouseOverBorder
        {
            get { return (Brush)GetValue(IBButtonMouseOverBorderProperty); }
            set { SetValue(IBButtonMouseOverBorderProperty, value); }
        }

        public static readonly DependencyProperty IBCloseButtonProperty = DependencyProperty.Register("IBCloseButton", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush IBCloseButton
        {
            get { return (Brush)GetValue(IBCloseButtonProperty); }
            set { SetValue(IBCloseButtonProperty, value); }
        }

        public static readonly DependencyProperty IBCloseButtonBorderProperty = DependencyProperty.Register("IBCloseButtonBorder", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush IBCloseButtonBorder
        {
            get { return (Brush)GetValue(IBCloseButtonBorderProperty); }
            set { SetValue(IBCloseButtonBorderProperty, value); }
        }

        public static readonly DependencyProperty IBCloseButtonDownProperty = DependencyProperty.Register("IBCloseButtonDown", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush IBCloseButtonDown
        {
            get { return (Brush)GetValue(IBCloseButtonDownProperty); }
            set { SetValue(IBCloseButtonDownProperty, value); }
        }

        public static readonly DependencyProperty IBCloseButtonDownBorderProperty = DependencyProperty.Register("IBCloseButtonDownBorder", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush IBCloseButtonDownBorder
        {
            get { return (Brush)GetValue(IBCloseButtonDownBorderProperty); }
            set { SetValue(IBCloseButtonDownBorderProperty, value); }
        }

        public static readonly DependencyProperty IBCloseButtonDownGlyphProperty = DependencyProperty.Register("IBCloseButtonDownGlyph", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush IBCloseButtonDownGlyph
        {
            get { return (Brush)GetValue(IBCloseButtonDownGlyphProperty); }
            set { SetValue(IBCloseButtonDownGlyphProperty, value); }
        }

        public static readonly DependencyProperty IBCloseButtonGlyphProperty = DependencyProperty.Register("IBCloseButtonGlyph", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush IBCloseButtonGlyph
        {
            get { return (Brush)GetValue(IBCloseButtonGlyphProperty); }
            set { SetValue(IBCloseButtonGlyphProperty, value); }
        }

        public static readonly DependencyProperty IBCloseButtonHoverProperty = DependencyProperty.Register("IBCloseButtonHover", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush IBCloseButtonHover
        {
            get { return (Brush)GetValue(IBCloseButtonHoverProperty); }
            set { SetValue(IBCloseButtonHoverProperty, value); }
        }

        public static readonly DependencyProperty IBCloseButtonHoverBorderProperty = DependencyProperty.Register("IBCloseButtonHoverBorder", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush IBCloseButtonHoverBorder
        {
            get { return (Brush)GetValue(IBCloseButtonHoverBorderProperty); }
            set { SetValue(IBCloseButtonHoverBorderProperty, value); }
        }

        public static readonly DependencyProperty IBCloseButtonHoverGlyphProperty = DependencyProperty.Register("IBCloseButtonHoverGlyph", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush IBCloseButtonHoverGlyph
        {
            get { return (Brush)GetValue(IBCloseButtonHoverGlyphProperty); }
            set { SetValue(IBCloseButtonHoverGlyphProperty, value); }
        }

        public static readonly DependencyProperty IBInfoBarBackgroundProperty = DependencyProperty.Register("IBInfoBarBackground", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush IBInfoBarBackground
        {
            get { return (Brush)GetValue(IBInfoBarBackgroundProperty); }
            set { SetValue(IBInfoBarBackgroundProperty, value); }
        }

        public static readonly DependencyProperty IBInfoBarBackgroundTextProperty = DependencyProperty.Register("IBInfoBarBackgroundText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush IBInfoBarBackgroundText
        {
            get { return (Brush)GetValue(IBInfoBarBackgroundTextProperty); }
            set { SetValue(IBInfoBarBackgroundTextProperty, value); }
        }

        public static readonly DependencyProperty IBInfoBarBorderProperty = DependencyProperty.Register("IBInfoBarBorder", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush IBInfoBarBorder
        {
            get { return (Brush)GetValue(IBInfoBarBorderProperty); }
            set { SetValue(IBInfoBarBorderProperty, value); }
        }
    }
}
