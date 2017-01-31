using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Microsoft.Templates.Wizard.ViewModels
{
    public static class Extensions
    {
        public const string DefaultProjectIcon = "pack://application:,,,/Microsoft.Templates.Wizard;component/Assets/DefaultProjectIcon.jpg";
        public static BitmapImage CreateIcon(string path)
        {
            Uri source;
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                source = new Uri(DefaultProjectIcon);
            }
            else
            {
                source = new Uri(path);
            }            
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            image.UriSource = source;
            image.EndInit();
            return image;
        }
    }
}
