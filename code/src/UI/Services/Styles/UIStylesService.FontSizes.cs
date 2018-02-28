// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Media;

namespace Microsoft.Templates.UI.Services
{
    public partial class UIStylesService
    {
        public static readonly DependencyProperty Environment100PercentFontSizeProperty = DependencyProperty.Register("Environment100PercentFontSize", typeof(double), typeof(UIStylesService), new PropertyMetadata(0.0));

        public double Environment100PercentFontSize
        {
            get { return (double)GetValue(Environment100PercentFontSizeProperty); }
            set { SetValue(Environment100PercentFontSizeProperty, value); }
        }

        public static readonly DependencyProperty Environment111PercentFontSizeProperty = DependencyProperty.Register("Environment111PercentFontSize", typeof(double), typeof(UIStylesService), new PropertyMetadata(0.0));

        public double Environment111PercentFontSize
        {
            get { return (double)GetValue(Environment111PercentFontSizeProperty); }
            set { SetValue(Environment111PercentFontSizeProperty, value); }
        }

        public static readonly DependencyProperty Environment122PercentFontSizeProperty = DependencyProperty.Register("Environment122PercentFontSize", typeof(double), typeof(UIStylesService), new PropertyMetadata(0.0));

        public double Environment122PercentFontSize
        {
            get { return (double)GetValue(Environment122PercentFontSizeProperty); }
            set { SetValue(Environment122PercentFontSizeProperty, value); }
        }

        public static readonly DependencyProperty Environment133PercentFontSizeProperty = DependencyProperty.Register("Environment133PercentFontSize", typeof(double), typeof(UIStylesService), new PropertyMetadata(0.0));

        public double Environment133PercentFontSize
        {
            get { return (double)GetValue(Environment133PercentFontSizeProperty); }
            set { SetValue(Environment133PercentFontSizeProperty, value); }
        }

        public static readonly DependencyProperty Environment155PercentFontSizeProperty = DependencyProperty.Register("Environment155PercentFontSize", typeof(double), typeof(UIStylesService), new PropertyMetadata(0.0));

        public double Environment155PercentFontSize
        {
            get { return (double)GetValue(Environment155PercentFontSizeProperty); }
            set { SetValue(Environment155PercentFontSizeProperty, value); }
        }

        public static readonly DependencyProperty Environment200PercentFontSizeProperty = DependencyProperty.Register("Environment200PercentFontSize", typeof(double), typeof(UIStylesService), new PropertyMetadata(0.0));

        public double Environment200PercentFontSize
        {
            get { return (double)GetValue(Environment200PercentFontSizeProperty); }
            set { SetValue(Environment200PercentFontSizeProperty, value); }
        }

        public static readonly DependencyProperty Environment310PercentFontSizeProperty = DependencyProperty.Register("Environment310PercentFontSize", typeof(double), typeof(UIStylesService), new PropertyMetadata(0.0));

        public double Environment310PercentFontSize
        {
            get { return (double)GetValue(Environment310PercentFontSizeProperty); }
            set { SetValue(Environment310PercentFontSizeProperty, value); }
        }

        public static readonly DependencyProperty Environment330PercentFontSizeProperty = DependencyProperty.Register("Environment330PercentFontSize", typeof(double), typeof(UIStylesService), new PropertyMetadata(0.0));

        public double Environment330PercentFontSize
        {
            get { return (double)GetValue(Environment330PercentFontSizeProperty); }
            set { SetValue(Environment330PercentFontSizeProperty, value); }
        }

        public static readonly DependencyProperty Environment375PercentFontSizeProperty = DependencyProperty.Register("Environment375PercentFontSize", typeof(double), typeof(UIStylesService), new PropertyMetadata(0.0));

        public double Environment375PercentFontSize
        {
            get { return (double)GetValue(Environment375PercentFontSizeProperty); }
            set { SetValue(Environment375PercentFontSizeProperty, value); }
        }

        public static readonly DependencyProperty EnvironmentFontFamilyProperty = DependencyProperty.Register("EnvironmentFontFamily", typeof(FontFamily), typeof(UIStylesService), new PropertyMetadata(null));

        public FontFamily EnvironmentFontFamily
        {
            get { return (FontFamily)GetValue(EnvironmentFontFamilyProperty); }
            set { SetValue(EnvironmentFontFamilyProperty, value); }
        }
    }
}
