// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Microsoft.Templates.UI.Extensions
{
    public class ImageExtensions
    {
        public static readonly DependencyProperty DisposableSourceProperty = DependencyProperty.RegisterAttached(
          "DisposableSource",
          typeof(string),
          typeof(ImageExtensions),
          new PropertyMetadata(null, OnDisposableSourcePropertyChanged));

        public static void SetDisposableSource(UIElement element, ImageSource value)
        {
            element.SetValue(DisposableSourceProperty, value);
        }

        public static string GetDisposableSource(UIElement element)
        {
            return (string)element.GetValue(DisposableSourceProperty);
        }

        private static void OnDisposableSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var image = d as Image;
            var source = GetDisposableSource(image);
            if (!string.IsNullOrEmpty(source))
            {
                var uriSource = new Uri(source);
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = uriSource;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                image.Source = bitmap;
            }
        }
    }
}
