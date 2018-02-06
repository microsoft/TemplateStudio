// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Media;

namespace Microsoft.Templates.UI.Services
{
    public partial class UIStylesService
    {
        public static readonly DependencyProperty WindowPanelProperty = DependencyProperty.Register("WindowPanel", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush WindowPanel
        {
            get { return (Brush)GetValue(WindowPanelProperty); }
            set { SetValue(WindowPanelProperty, value); }
        }

        public static readonly DependencyProperty WindowPanelTextProperty = DependencyProperty.Register("WindowPanelText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush WindowPanelText
        {
            get { return (Brush)GetValue(WindowPanelTextProperty); }
            set { SetValue(WindowPanelTextProperty, value); }
        }

        public static readonly DependencyProperty WindowBorderProperty = DependencyProperty.Register("WindowBorder", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush WindowBorder
        {
            get { return (Brush)GetValue(WindowBorderProperty); }
            set { SetValue(WindowBorderProperty, value); }
        }

        public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register("HeaderText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush HeaderText
        {
            get { return (Brush)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }

        public static readonly DependencyProperty HeaderTextSecondaryProperty = DependencyProperty.Register("HeaderTextSecondary", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush HeaderTextSecondary
        {
            get { return (Brush)GetValue(HeaderTextSecondaryProperty); }
            set { SetValue(HeaderTextSecondaryProperty, value); }
        }

        public static readonly DependencyProperty HyperlinkProperty = DependencyProperty.Register("Hyperlink", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush Hyperlink
        {
            get { return (Brush)GetValue(HyperlinkProperty); }
            set { SetValue(HyperlinkProperty, value); }
        }

        public static readonly DependencyProperty HyperlinkHoverProperty = DependencyProperty.Register("HyperlinkHover", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush HyperlinkHover
        {
            get { return (Brush)GetValue(HyperlinkHoverProperty); }
            set { SetValue(HyperlinkHoverProperty, value); }
        }

        public static readonly DependencyProperty HyperlinkPressedProperty = DependencyProperty.Register("HyperlinkPressed", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush HyperlinkPressed
        {
            get { return (Brush)GetValue(HyperlinkPressedProperty); }
            set { SetValue(HyperlinkPressedProperty, value); }
        }

        public static readonly DependencyProperty HyperlinkDisabledProperty = DependencyProperty.Register("HyperlinkDisabled", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush HyperlinkDisabled
        {
            get { return (Brush)GetValue(HyperlinkDisabledProperty); }
            set { SetValue(HyperlinkDisabledProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemActiveProperty = DependencyProperty.Register("SelectedItemActive", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush SelectedItemActive
        {
            get { return (Brush)GetValue(SelectedItemActiveProperty); }
            set { SetValue(SelectedItemActiveProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemInactiveProperty = DependencyProperty.Register("SelectedItemInactive", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush SelectedItemInactive
        {
            get { return (Brush)GetValue(SelectedItemInactiveProperty); }
            set { SetValue(SelectedItemInactiveProperty, value); }
        }

        public static readonly DependencyProperty ListItemMouseOverProperty = DependencyProperty.Register("ListItemMouseOver", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ListItemMouseOver
        {
            get { return (Brush)GetValue(ListItemMouseOverProperty); }
            set { SetValue(ListItemMouseOverProperty, value); }
        }

        public static readonly DependencyProperty ListItemDisabledTextProperty = DependencyProperty.Register("ListItemDisabledText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ListItemDisabledText
        {
            get { return (Brush)GetValue(ListItemDisabledTextProperty); }
            set { SetValue(ListItemDisabledTextProperty, value); }
        }

        public static readonly DependencyProperty GridHeadingBackgroundProperty = DependencyProperty.Register("GridHeadingBackground", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush GridHeadingBackground
        {
            get { return (Brush)GetValue(GridHeadingBackgroundProperty); }
            set { SetValue(GridHeadingBackgroundProperty, value); }
        }

        public static readonly DependencyProperty GridHeadingHoverBackgroundProperty = DependencyProperty.Register("GridHeadingHoverBackground", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush GridHeadingHoverBackground
        {
            get { return (Brush)GetValue(GridHeadingHoverBackgroundProperty); }
            set { SetValue(GridHeadingHoverBackgroundProperty, value); }
        }

        public static readonly DependencyProperty GridHeadingTextProperty = DependencyProperty.Register("GridHeadingText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush GridHeadingText
        {
            get { return (Brush)GetValue(GridHeadingTextProperty); }
            set { SetValue(GridHeadingTextProperty, value); }
        }

        public static readonly DependencyProperty GridHeadingHoverTextProperty = DependencyProperty.Register("GridHeadingHoverText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush GridHeadingHoverText
        {
            get { return (Brush)GetValue(GridHeadingHoverTextProperty); }
            set { SetValue(GridHeadingHoverTextProperty, value); }
        }

        public static readonly DependencyProperty GridLineProperty = DependencyProperty.Register("GridLine", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush GridLine
        {
            get { return (Brush)GetValue(GridLineProperty); }
            set { SetValue(GridLineProperty, value); }
        }

        public static readonly DependencyProperty SectionDividerProperty = DependencyProperty.Register("SectionDivider", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush SectionDivider
        {
            get { return (Brush)GetValue(SectionDividerProperty); }
            set { SetValue(SectionDividerProperty, value); }
        }

        public static readonly DependencyProperty WindowButtonProperty = DependencyProperty.Register("WindowButton", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush WindowButton
        {
            get { return (Brush)GetValue(WindowButtonProperty); }
            set { SetValue(WindowButtonProperty, value); }
        }

        public static readonly DependencyProperty WindowButtonHoverProperty = DependencyProperty.Register("WindowButtonHover", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush WindowButtonHover
        {
            get { return (Brush)GetValue(WindowButtonHoverProperty); }
            set { SetValue(WindowButtonHoverProperty, value); }
        }

        public static readonly DependencyProperty WindowButtonDownProperty = DependencyProperty.Register("WindowButtonDown", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush WindowButtonDown
        {
            get { return (Brush)GetValue(WindowButtonDownProperty); }
            set { SetValue(WindowButtonDownProperty, value); }
        }

        public static readonly DependencyProperty WindowButtonBorderProperty = DependencyProperty.Register("WindowButtonBorder", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush WindowButtonBorder
        {
            get { return (Brush)GetValue(WindowButtonBorderProperty); }
            set { SetValue(WindowButtonBorderProperty, value); }
        }

        public static readonly DependencyProperty WindowButtonHoverBorderProperty = DependencyProperty.Register("WindowButtonHoverBorder", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush WindowButtonHoverBorder
        {
            get { return (Brush)GetValue(WindowButtonHoverBorderProperty); }
            set { SetValue(WindowButtonHoverBorderProperty, value); }
        }

        public static readonly DependencyProperty WindowButtonDownBorderProperty = DependencyProperty.Register("WindowButtonDownBorder", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush WindowButtonDownBorder
        {
            get { return (Brush)GetValue(WindowButtonDownBorderProperty); }
            set { SetValue(WindowButtonDownBorderProperty, value); }
        }

        public static readonly DependencyProperty WindowButtonGlyphProperty = DependencyProperty.Register("WindowButtonGlyph", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush WindowButtonGlyph
        {
            get { return (Brush)GetValue(WindowButtonGlyphProperty); }
            set { SetValue(WindowButtonGlyphProperty, value); }
        }

        public static readonly DependencyProperty WindowButtonHoverGlyphProperty = DependencyProperty.Register("WindowButtonHoverGlyph", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush WindowButtonHoverGlyph
        {
            get { return (Brush)GetValue(WindowButtonHoverGlyphProperty); }
            set { SetValue(WindowButtonHoverGlyphProperty, value); }
        }

        public static readonly DependencyProperty WindowButtonDownGlyphProperty = DependencyProperty.Register("WindowButtonDownGlyph", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush WindowButtonDownGlyph
        {
            get { return (Brush)GetValue(WindowButtonDownGlyphProperty); }
            set { SetValue(WindowButtonDownGlyphProperty, value); }
        }

        public static readonly DependencyProperty WizardFooterProperty = DependencyProperty.Register("WizardFooter", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush WizardFooter
        {
            get { return (Brush)GetValue(WizardFooterProperty); }
            set { SetValue(WizardFooterProperty, value); }
        }

        public static readonly DependencyProperty WizardFooterTextProperty = DependencyProperty.Register("WizardFooterText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush WizardFooterText
        {
            get { return (Brush)GetValue(WizardFooterTextProperty); }
            set { SetValue(WizardFooterTextProperty, value); }
        }

        public static readonly DependencyProperty ListItemTextProperty = DependencyProperty.Register("ListItemText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ListItemText
        {
            get { return (Brush)GetValue(ListItemTextProperty); }
            set { SetValue(ListItemTextProperty, value); }
        }

        public static readonly DependencyProperty ListItemTextDisabledProperty = DependencyProperty.Register("ListItemTextDisabled", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush ListItemTextDisabled
        {
            get { return (Brush)GetValue(ListItemTextDisabledProperty); }
            set { SetValue(ListItemTextDisabledProperty, value); }
        }
    }
}
