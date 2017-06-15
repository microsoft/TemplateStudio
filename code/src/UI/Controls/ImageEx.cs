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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Shapes = System.Windows.Shapes;

namespace Microsoft.Templates.UI.Controls
{
    public class ImageEx : ContentControl
    {
        private const string XamlExtension = ".xaml";

        public override void OnApplyTemplate()
        {
            if (string.IsNullOrWhiteSpace(SourcePath))
            {
                Content = CreateFromBitmap();
            }

            var sourceExtension = Path.GetExtension(SourcePath);

            if (File.Exists(SourcePath) && sourceExtension?.Equals(XamlExtension, StringComparison.OrdinalIgnoreCase) == true)
            {
                Content = CreateFromXaml();
            }
            else
            {
                Content = CreateFromBitmap();
            }
        }

        private UIElement CreateFromBitmap()
        {
            var bitmap = CreateIcon(SourcePath);
            if (bitmap == null)
            {
                return null;
            }

            var image = new Image
            {
                Source = bitmap,
                Stretch = Stretch
            };

            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);

            return image;
        }

        private UIElement CreateFromXaml()
        {
            using (var sr = new StreamReader(SourcePath))
            {
                var element = XamlReader.Load(sr.BaseStream) as UIElement;

                var paths = element
                                .ChildrenOfType<Shapes.Path>()
                                .ToList();

                if (paths.Count > 0)
                {
                    paths.ForEach(p => BindingOperations.SetBinding(p, Shapes.Path.FillProperty, CreateBinding(this, nameof(Foreground))));
                }
                else
                {
                    var shapes = element
                                    .ChildrenOfType<Shapes.Shape>(true)
                                    .ToList();

                    shapes.ForEach(s => BindingOperations.SetBinding(s, Shapes.Shape.StrokeProperty, CreateBinding(this, nameof(Foreground))));
                }

                return element;
            }
        }

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(ImageEx), new PropertyMetadata(Stretch.Uniform));
        public Stretch Stretch
        {
            get => (Stretch)GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }

        public static readonly DependencyProperty SourcePathProperty = DependencyProperty.Register("SourcePath", typeof(string), typeof(ImageEx), new PropertyMetadata(null));
        public string SourcePath
        {
            get => (string)GetValue(SourcePathProperty);
            set => SetValue(SourcePathProperty, value);
        }

        public static readonly DependencyProperty FallbackImageProperty = DependencyProperty.Register("FallbackImage", typeof(ImageSource), typeof(ImageEx), new PropertyMetadata(null));
        public ImageSource FallbackImage
        {
            get => (ImageSource)GetValue(FallbackImageProperty);
            set => SetValue(FallbackImageProperty, value);
        }

        public ImageSource CreateIcon(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                {
                    return FallbackImage;
                }
                else
                {
                    return CreateBitMap(new Uri(path));
                }
            }
            catch (IOException)
            {
                // SYNC AT SAME TIME IS LOADING THE ICON OR ICON IS LOCKED
                return FallbackImage;
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

        protected static Binding CreateBinding(object source, string path)
        {
            return new Binding
            {
                Path = new PropertyPath(path),
                Source = source
            };
        }
    }
}
