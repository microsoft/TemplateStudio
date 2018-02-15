// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Media;

namespace Microsoft.Templates.UI.Services
{
    public partial class UIStylesService
    {
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
