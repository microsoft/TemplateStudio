// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace Microsoft.Templates.UI.Controls
{
    public class ImageEx : ContentControl
    {
        private const string XamlExtension = ".xaml";

        public override void OnApplyTemplate()
        {
            if (string.IsNullOrWhiteSpace(SourcePath))
            {
                return;
            }

            var sourceExtension = Path.GetExtension(SourcePath);

            if (File.Exists(SourcePath) && sourceExtension?.Equals(XamlExtension, StringComparison.OrdinalIgnoreCase) == true)
            {
                using (var sr = new StreamReader(SourcePath))
                {
                    Content = XamlReader.Load(sr.BaseStream) as UIElement;
                }
            }
            else
            {
                var bitmap = CreateIcon(SourcePath);
                if (bitmap == null)
                {
                    return;
                }

                var image = new Image
                {
                    Source = bitmap
                };

                Content = image;
            }
        }

        public static readonly DependencyProperty SourcePathProperty = DependencyProperty.Register("SourcePath", typeof(string), typeof(ImageEx), new PropertyMetadata(null));
        public string SourcePath
        {
            get { return (string)GetValue(SourcePathProperty); }
            set { SetValue(SourcePathProperty, value); }
        }

        public static readonly DependencyProperty FallbackImageProperty = DependencyProperty.Register("FallbackImage", typeof(string), typeof(ImageEx), new PropertyMetadata(null));
        public string FallbackImage
        {
            get { return (string)GetValue(FallbackImageProperty); }
            set { SetValue(FallbackImageProperty, value); }
        }

        public BitmapImage CreateIcon(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                {
                    return CreateFallback();
                }
                else
                {
                    return CreateBitMap(new Uri(path));
                }
            }
            catch (IOException)
            {
                //SYNC AT SAME TIME IS LOADING THE ICON OR ICON IS LOCKED
                return CreateFallback();
            }
        }

        private static BitmapImage CreateBitMap(Uri source)
        {
            var image = new BitmapImage();

            image.BeginInit();

            image.CacheOption = BitmapCacheOption.OnLoad;
            image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            image.UriSource = source;

            image.EndInit();

            return image;
        }

        private BitmapImage CreateFallback()
        {
            if (string.IsNullOrEmpty(FallbackImage))
            {
                return null;
            }

            return CreateBitMap(new Uri(FallbackImage));
        }
    }
}
