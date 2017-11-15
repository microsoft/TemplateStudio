// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Media;

namespace Microsoft.Templates.UI.V2Services
{
    public partial class UIStylesService
    {
        public static readonly DependencyProperty EnvironmentBackgroundColorProperty = DependencyProperty.Register("EnvironmentBackgroundColor", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush EnvironmentBackgroundColor
        {
            get { return (Brush)GetValue(EnvironmentBackgroundColorProperty); }
            set { SetValue(EnvironmentBackgroundColorProperty, value); }
        }

        public static readonly DependencyProperty EnvironmentBackgroundTexture1ColorProperty = DependencyProperty.Register("EnvironmentBackgroundTexture1Color", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush EnvironmentBackgroundTexture1Color
        {
            get { return (Brush)GetValue(EnvironmentBackgroundTexture1ColorProperty); }
            set { SetValue(EnvironmentBackgroundTexture1ColorProperty, value); }
        }

        public static readonly DependencyProperty EnvironmentBackgroundTexture2ColorProperty = DependencyProperty.Register("EnvironmentBackgroundTexture2Color", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush EnvironmentBackgroundTexture2Color
        {
            get { return (Brush)GetValue(EnvironmentBackgroundTexture2ColorProperty); }
            set { SetValue(EnvironmentBackgroundTexture2ColorProperty, value); }
        }

        public static readonly DependencyProperty XXXXXXXXXXProperty = DependencyProperty.Register("XXXXXXXXXX", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush XXXXXXXXXX
        {
            get { return (Brush)GetValue(XXXXXXXXXXProperty); }
            set { SetValue(XXXXXXXXXXProperty, value); }
        }
    }
}
