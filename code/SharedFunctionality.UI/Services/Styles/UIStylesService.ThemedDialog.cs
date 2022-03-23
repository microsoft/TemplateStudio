// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Media;

namespace Microsoft.Templates.UI.Services
{
    public partial class UIStylesService
    {
        public static readonly DependencyProperty TDListBoxTextProperty = DependencyProperty.Register("TDListBoxText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush TDListBoxText
        {
            get { return (Brush)GetValue(TDListBoxTextProperty); }
            set { SetValue(TDListBoxTextProperty, value); }
        }

        public static readonly DependencyProperty TDSelectedItemActiveProperty = DependencyProperty.Register("TDSelectedItemActive", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush TDSelectedItemActive
        {
            get { return (Brush)GetValue(TDSelectedItemActiveProperty); }
            set { SetValue(TDSelectedItemActiveProperty, value); }
        }

        public static readonly DependencyProperty TDSelectedItemActiveTextProperty = DependencyProperty.Register("TDSelectedItemActiveText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush TDSelectedItemActiveText
        {
            get { return (Brush)GetValue(TDSelectedItemActiveTextProperty); }
            set { SetValue(TDSelectedItemActiveTextProperty, value); }
        }

        public static readonly DependencyProperty TDListItemMouseOverProperty = DependencyProperty.Register("TDListItemMouseOver", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush TDListItemMouseOver
        {
            get { return (Brush)GetValue(TDListItemMouseOverProperty); }
            set { SetValue(TDListItemMouseOverProperty, value); }
        }

        public static readonly DependencyProperty TDListItemMouseOverTextProperty = DependencyProperty.Register("TDListItemMouseOverText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush TDListItemMouseOverText
        {
            get { return (Brush)GetValue(TDListItemMouseOverTextProperty); }
            set { SetValue(TDListItemMouseOverTextProperty, value); }
        }

        public static readonly DependencyProperty TDListItemDisabledTextProperty = DependencyProperty.Register("TDListItemDisabledText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush TDListItemDisabledText
        {
            get { return (Brush)GetValue(TDListItemDisabledTextProperty); }
            set { SetValue(TDListItemDisabledTextProperty, value); }
        }
    }
}
