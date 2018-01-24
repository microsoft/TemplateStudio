// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Media;

namespace Microsoft.Templates.UI.V2Services
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
    }
}
