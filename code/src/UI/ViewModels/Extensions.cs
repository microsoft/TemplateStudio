using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Microsoft.Templates.UI.ViewModels
{
    public static class Extensions
    {
        public const string DefaultProjectIcon = "pack://application:,,,/Microsoft.Templates.Wizard;component/Assets/DefaultProjectIcon.png";
        public static BitmapImage CreateIcon(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                {
                    return CreateBitMap(new Uri(DefaultProjectIcon));
                }
                else
                {
                    return CreateBitMap(new Uri(path));
                }
            }
            catch (IOException)
            {
                //SYNC AT SAME TIME IS LOADING THE ICON OR ICON IS LOCKED
                return CreateBitMap(new Uri(DefaultProjectIcon));
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
    }
}
