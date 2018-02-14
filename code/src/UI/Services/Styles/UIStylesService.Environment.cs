// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Media;

namespace Microsoft.Templates.UI.Services
{
    public partial class UIStylesService
    {
        // Expander Colors
        public static readonly DependencyProperty PageSideBarExpanderBodyProperty = DependencyProperty.Register("PageSideBarExpanderBody", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush PageSideBarExpanderBody
        {
            get { return (Brush)GetValue(PageSideBarExpanderBodyProperty); }
            set { SetValue(PageSideBarExpanderBodyProperty, value); }
        }

        public static readonly DependencyProperty PageSideBarExpanderChevronProperty = DependencyProperty.Register("PageSideBarExpanderChevron", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush PageSideBarExpanderChevron
        {
            get { return (Brush)GetValue(PageSideBarExpanderChevronProperty); }
            set { SetValue(PageSideBarExpanderChevronProperty, value); }
        }

        public static readonly DependencyProperty PageSideBarExpanderHeaderProperty = DependencyProperty.Register("PageSideBarExpanderHeader", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush PageSideBarExpanderHeader
        {
            get { return (Brush)GetValue(PageSideBarExpanderHeaderProperty); }
            set { SetValue(PageSideBarExpanderHeaderProperty, value); }
        }

        public static readonly DependencyProperty PageSideBarExpanderHeaderHoverProperty = DependencyProperty.Register("PageSideBarExpanderHeaderHover", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush PageSideBarExpanderHeaderHover
        {
            get { return (Brush)GetValue(PageSideBarExpanderHeaderHoverProperty); }
            set { SetValue(PageSideBarExpanderHeaderHoverProperty, value); }
        }

        public static readonly DependencyProperty PageSideBarExpanderHeaderPressedProperty = DependencyProperty.Register("PageSideBarExpanderHeaderPressed", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush PageSideBarExpanderHeaderPressed
        {
            get { return (Brush)GetValue(PageSideBarExpanderHeaderPressedProperty); }
            set { SetValue(PageSideBarExpanderHeaderPressedProperty, value); }
        }

        public static readonly DependencyProperty PageSideBarExpanderSeparatorProperty = DependencyProperty.Register("PageSideBarExpanderSeparator", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush PageSideBarExpanderSeparator
        {
            get { return (Brush)GetValue(PageSideBarExpanderSeparatorProperty); }
            set { SetValue(PageSideBarExpanderSeparatorProperty, value); }
        }

        public static readonly DependencyProperty PageSideBarExpanderTextProperty = DependencyProperty.Register("PageSideBarExpanderText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush PageSideBarExpanderText
        {
            get { return (Brush)GetValue(PageSideBarExpanderTextProperty); }
            set { SetValue(PageSideBarExpanderTextProperty, value); }
        }

        public static readonly DependencyProperty ScrollBarArrowBackgroundProperty = DependencyProperty.Register("ScrollBarArrowBackground", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ScrollBarArrowBackground
        {
            get { return (Brush)GetValue(ScrollBarArrowBackgroundProperty); }
            set { SetValue(ScrollBarArrowBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ScrollBarArrowDisabledBackgroundProperty = DependencyProperty.Register("ScrollBarArrowDisabledBackground", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ScrollBarArrowDisabledBackground
        {
            get { return (Brush)GetValue(ScrollBarArrowDisabledBackgroundProperty); }
            set { SetValue(ScrollBarArrowDisabledBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ScrollBarArrowGlyphProperty = DependencyProperty.Register("ScrollBarArrowGlyph", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ScrollBarArrowGlyph
        {
            get { return (Brush)GetValue(ScrollBarArrowGlyphProperty); }
            set { SetValue(ScrollBarArrowGlyphProperty, value); }
        }

        public static readonly DependencyProperty ScrollBarArrowGlyphDisabledProperty = DependencyProperty.Register("ScrollBarArrowGlyphDisabled", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ScrollBarArrowGlyphDisabled
        {
            get { return (Brush)GetValue(ScrollBarArrowGlyphDisabledProperty); }
            set { SetValue(ScrollBarArrowGlyphDisabledProperty, value); }
        }

        public static readonly DependencyProperty ScrollBarArrowGlyphMouseOverProperty = DependencyProperty.Register("ScrollBarArrowGlyphMouseOver", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ScrollBarArrowGlyphMouseOver
        {
            get { return (Brush)GetValue(ScrollBarArrowGlyphMouseOverProperty); }
            set { SetValue(ScrollBarArrowGlyphMouseOverProperty, value); }
        }

        public static readonly DependencyProperty ScrollBarArrowGlyphPressedProperty = DependencyProperty.Register("ScrollBarArrowGlyphPressed", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ScrollBarArrowGlyphPressed
        {
            get { return (Brush)GetValue(ScrollBarArrowGlyphPressedProperty); }
            set { SetValue(ScrollBarArrowGlyphPressedProperty, value); }
        }

        public static readonly DependencyProperty ScrollBarArrowMouseOverBackgroundProperty = DependencyProperty.Register("ScrollBarArrowMouseOverBackground", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ScrollBarArrowMouseOverBackground
        {
            get { return (Brush)GetValue(ScrollBarArrowMouseOverBackgroundProperty); }
            set { SetValue(ScrollBarArrowMouseOverBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ScrollBarArrowPressedBackgroundProperty = DependencyProperty.Register("ScrollBarArrowPressedBackground", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ScrollBarArrowPressedBackground
        {
            get { return (Brush)GetValue(ScrollBarArrowPressedBackgroundProperty); }
            set { SetValue(ScrollBarArrowPressedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ScrollBarBackgroundProperty = DependencyProperty.Register("ScrollBarBackground", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ScrollBarBackground
        {
            get { return (Brush)GetValue(ScrollBarBackgroundProperty); }
            set { SetValue(ScrollBarBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ScrollBarBorderProperty = DependencyProperty.Register("ScrollBarBorder", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ScrollBarBorder
        {
            get { return (Brush)GetValue(ScrollBarBorderProperty); }
            set { SetValue(ScrollBarBorderProperty, value); }
        }

        public static readonly DependencyProperty ScrollBarDisabledBackgroundProperty = DependencyProperty.Register("ScrollBarDisabledBackground", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ScrollBarDisabledBackground
        {
            get { return (Brush)GetValue(ScrollBarDisabledBackgroundProperty); }
            set { SetValue(ScrollBarDisabledBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ScrollBarThumbBackgroundProperty = DependencyProperty.Register("ScrollBarThumbBackground", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ScrollBarThumbBackground
        {
            get { return (Brush)GetValue(ScrollBarThumbBackgroundProperty); }
            set { SetValue(ScrollBarThumbBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ScrollBarThumbBorderProperty = DependencyProperty.Register("ScrollBarThumbBorder", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ScrollBarThumbBorder
        {
            get { return (Brush)GetValue(ScrollBarThumbBorderProperty); }
            set { SetValue(ScrollBarThumbBorderProperty, value); }
        }

        public static readonly DependencyProperty ScrollBarThumbDisabledProperty = DependencyProperty.Register("ScrollBarThumbDisabled", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ScrollBarThumbDisabled
        {
            get { return (Brush)GetValue(ScrollBarThumbDisabledProperty); }
            set { SetValue(ScrollBarThumbDisabledProperty, value); }
        }

        public static readonly DependencyProperty ScrollBarThumbGlyphProperty = DependencyProperty.Register("ScrollBarThumbGlyph", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ScrollBarThumbGlyph
        {
            get { return (Brush)GetValue(ScrollBarThumbGlyphProperty); }
            set { SetValue(ScrollBarThumbGlyphProperty, value); }
        }

        public static readonly DependencyProperty ScrollBarThumbGlyphMouseOverBorderProperty = DependencyProperty.Register("ScrollBarThumbGlyphMouseOverBorder", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ScrollBarThumbGlyphMouseOverBorder
        {
            get { return (Brush)GetValue(ScrollBarThumbGlyphMouseOverBorderProperty); }
            set { SetValue(ScrollBarThumbGlyphMouseOverBorderProperty, value); }
        }

        public static readonly DependencyProperty ScrollBarThumbGlyphPressedBorderProperty = DependencyProperty.Register("ScrollBarThumbGlyphPressedBorder", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ScrollBarThumbGlyphPressedBorder
        {
            get { return (Brush)GetValue(ScrollBarThumbGlyphPressedBorderProperty); }
            set { SetValue(ScrollBarThumbGlyphPressedBorderProperty, value); }
        }

        public static readonly DependencyProperty ScrollBarThumbMouseOverBackgroundProperty = DependencyProperty.Register("ScrollBarThumbMouseOverBackground", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ScrollBarThumbMouseOverBackground
        {
            get { return (Brush)GetValue(ScrollBarThumbMouseOverBackgroundProperty); }
            set { SetValue(ScrollBarThumbMouseOverBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ScrollBarThumbMouseOverBorderProperty = DependencyProperty.Register("ScrollBarThumbMouseOverBorder", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ScrollBarThumbMouseOverBorder
        {
            get { return (Brush)GetValue(ScrollBarThumbMouseOverBorderProperty); }
            set { SetValue(ScrollBarThumbMouseOverBorderProperty, value); }
        }

        public static readonly DependencyProperty ScrollBarThumbPressedBackgroundProperty = DependencyProperty.Register("ScrollBarThumbPressedBackground", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ScrollBarThumbPressedBackground
        {
            get { return (Brush)GetValue(ScrollBarThumbPressedBackgroundProperty); }
            set { SetValue(ScrollBarThumbPressedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ScrollBarThumbPressedBorderProperty = DependencyProperty.Register("ScrollBarThumbPressedBorder", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ScrollBarThumbPressedBorder
        {
            get { return (Brush)GetValue(ScrollBarThumbPressedBorderProperty); }
            set { SetValue(ScrollBarThumbPressedBorderProperty, value); }
        }
    }
}
