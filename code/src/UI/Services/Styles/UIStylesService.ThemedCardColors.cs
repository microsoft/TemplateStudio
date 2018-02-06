// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Media;

namespace Microsoft.Templates.UI.Services
{
    public partial class UIStylesService
    {
        public static readonly DependencyProperty CardTitleTextProperty = DependencyProperty.Register("CardTitleText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CardTitleText
        {
            get { return (Brush)GetValue(CardTitleTextProperty); }
            set { SetValue(CardTitleTextProperty, value); }
        }

        public static readonly DependencyProperty CardDescriptionTextProperty = DependencyProperty.Register("CardDescriptionText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CardDescriptionText
        {
            get { return (Brush)GetValue(CardDescriptionTextProperty); }
            set { SetValue(CardDescriptionTextProperty, value); }
        }

        public static readonly DependencyProperty CardIconProperty = DependencyProperty.Register("CardIcon", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CardIcon
        {
            get { return (Brush)GetValue(CardIconProperty); }
            set { SetValue(CardIconProperty, value); }
        }

        public static readonly DependencyProperty CardFooterTextProperty = DependencyProperty.Register("CardFooterText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CardFooterText
        {
            get { return (Brush)GetValue(CardFooterTextProperty); }
            set { SetValue(CardFooterTextProperty, value); }
        }

        public static readonly DependencyProperty CardBackgroundDefaultProperty = DependencyProperty.Register("CardBackgroundDefault", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CardBackgroundDefault
        {
            get { return (Brush)GetValue(CardBackgroundDefaultProperty); }
            set { SetValue(CardBackgroundDefaultProperty, value); }
        }

        public static readonly DependencyProperty CardBackgroundFocusProperty = DependencyProperty.Register("CardBackgroundFocus", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CardBackgroundFocus
        {
            get { return (Brush)GetValue(CardBackgroundFocusProperty); }
            set { SetValue(CardBackgroundFocusProperty, value); }
        }

        public static readonly DependencyProperty CardBackgroundHoverProperty = DependencyProperty.Register("CardBackgroundHover", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CardBackgroundHover
        {
            get { return (Brush)GetValue(CardBackgroundHoverProperty); }
            set { SetValue(CardBackgroundHoverProperty, value); }
        }

        public static readonly DependencyProperty CardBackgroundPressedProperty = DependencyProperty.Register("CardBackgroundPressed", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CardBackgroundPressed
        {
            get { return (Brush)GetValue(CardBackgroundPressedProperty); }
            set { SetValue(CardBackgroundPressedProperty, value); }
        }

        public static readonly DependencyProperty CardBackgroundSelectedProperty = DependencyProperty.Register("CardBackgroundSelected", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CardBackgroundSelected
        {
            get { return (Brush)GetValue(CardBackgroundSelectedProperty); }
            set { SetValue(CardBackgroundSelectedProperty, value); }
        }

        public static readonly DependencyProperty CardBackgroundDisabledProperty = DependencyProperty.Register("CardBackgroundDisabled", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CardBackgroundDisabled
        {
            get { return (Brush)GetValue(CardBackgroundDisabledProperty); }
            set { SetValue(CardBackgroundDisabledProperty, value); }
        }

        public static readonly DependencyProperty CardBorderDefaultProperty = DependencyProperty.Register("CardBorderDefault", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CardBorderDefault
        {
            get { return (Brush)GetValue(CardBorderDefaultProperty); }
            set { SetValue(CardBorderDefaultProperty, value); }
        }

        public static readonly DependencyProperty CardBorderFocusProperty = DependencyProperty.Register("CardBorderFocus", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CardBorderFocus
        {
            get { return (Brush)GetValue(CardBorderFocusProperty); }
            set { SetValue(CardBorderFocusProperty, value); }
        }

        public static readonly DependencyProperty CardBorderHoverProperty = DependencyProperty.Register("CardBorderHover", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CardBorderHover
        {
            get { return (Brush)GetValue(CardBorderHoverProperty); }
            set { SetValue(CardBorderHoverProperty, value); }
        }

        public static readonly DependencyProperty CardBorderPressedProperty = DependencyProperty.Register("CardBorderPressed", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CardBorderPressed
        {
            get { return (Brush)GetValue(CardBorderPressedProperty); }
            set { SetValue(CardBorderPressedProperty, value); }
        }

        public static readonly DependencyProperty CardBorderSelectedProperty = DependencyProperty.Register("CardBorderSelected", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CardBorderSelected
        {
            get { return (Brush)GetValue(CardBorderSelectedProperty); }
            set { SetValue(CardBorderSelectedProperty, value); }
        }

        public static readonly DependencyProperty CardBorderDisabledProperty = DependencyProperty.Register("CardBorderDisabled", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush CardBorderDisabled
        {
            get { return (Brush)GetValue(CardBorderDisabledProperty); }
            set { SetValue(CardBorderDisabledProperty, value); }
        }
    }
}
