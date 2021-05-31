// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
        private bool _isInitialized;

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(ImageEx), new PropertyMetadata(Stretch.Uniform));

        public Stretch Stretch
        {
            get => (Stretch)GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }

        public static readonly DependencyProperty SourcePathProperty = DependencyProperty.Register("SourcePath", typeof(string), typeof(ImageEx), new PropertyMetadata(null, OnSourcePathPropertyChanged));

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

        public ImageEx()
        {
            Focusable = false;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == ForegroundProperty)
            {
                if (_isInitialized && Content != null && !IsXamlImage)
                {
                    ChangeColorOfBitmap(Content, (Foreground as SolidColorBrush).Color);
                }
            }
        }

        public override void OnApplyTemplate()
        {
            _isInitialized = true;
            Initialize();
        }

        private void Initialize()
        {
            if (!_isInitialized)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(SourcePath))
            {
                Content = CreateFromBitmap();
            }

            if (IsXamlImage)
            {
                Content = CreateFromXaml();
            }
            else
            {
                Content = CreateFromBitmap();
                ChangeColorOfBitmap(Content, (Foreground as SolidColorBrush).Color);
            }
        }

        private bool IsXamlImage
        {
            get
            {
                var sourceExtension = Path.GetExtension(SourcePath);
                return File.Exists(SourcePath) && sourceExtension?.Equals(XamlExtension, StringComparison.OrdinalIgnoreCase) == true;
            }
        }

        private static void ChangeColorOfBitmap(object sender, Color setColor)
        {
            var writeableBmp = (sender as Image).Source as WriteableBitmap;

            using (writeableBmp.GetBitmapContext())
            {
                writeableBmp.ForEach((x, y, color) => Color.FromArgb(color.A, setColor.R, setColor.G, setColor.B));
            }
        }

        private UIElement CreateFromBitmap()
        {
            var bitmap = new WriteableBitmap(CreateIcon(SourcePath) as BitmapSource);

            if (bitmap == null)
            {
                return null;
            }

            var image = new Image
            {
                Source = bitmap,
                Stretch = Stretch,
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

                if (paths.Any())
                {
                    paths.ForEach(p => BindingOperations.SetBinding(p, Shapes.Shape.FillProperty, CreateBinding(this, nameof(Foreground))));
                }

                var polygons = element
                                .ChildrenOfType<Shapes.Polygon>()
                                .ToList();

                if (polygons.Any())
                {
                    polygons.ForEach(p => BindingOperations.SetBinding(p, Shapes.Shape.FillProperty, CreateBinding(this, nameof(Foreground))));
                }

                var shapes = element
                                .ChildrenOfType<Shapes.Shape>(true)
                                .ToList();

                foreach (var shape in shapes)
                {
                    BindingOperations.SetBinding(
                        shape,
                        shape.StrokeThickness > 0 ? Shapes.Shape.StrokeProperty : Shapes.Shape.FillProperty,
                        CreateBinding(this, nameof(Foreground)));
                }

                return element;
            }
        }

        private ImageSource CreateIcon(string path)
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
                Source = source,
            };
        }

        private static void OnSourcePathPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ImageEx control)
            {
                control.Initialize();
            }
        }
    }
}
