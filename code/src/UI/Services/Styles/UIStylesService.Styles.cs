// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Media;

namespace Microsoft.Templates.UI.Services
{
    public partial class UIStylesService
    {
        public static readonly DependencyProperty FocusVisualStyleProperty = DependencyProperty.Register("FocusVisualStyleProperty", typeof(Style), typeof(UIStylesService), new PropertyMetadata(null));

        public Style FocusVisualStyle
        {
            get { return (Style)GetValue(FocusVisualStyleProperty); }
            set { SetValue(FocusVisualStyleProperty, value); }
        }
    }
}
