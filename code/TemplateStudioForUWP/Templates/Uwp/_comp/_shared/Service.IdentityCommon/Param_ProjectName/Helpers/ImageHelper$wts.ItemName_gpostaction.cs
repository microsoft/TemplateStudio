//{[{
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;
//}]}

namespace Param_RootNamespace.Helpers
{
    public static class ImageHelper
    {
//^^
//{[{

        public static async Task<BitmapImage> ImageFromStringAsync(string data)
        {
            var byteArray = Convert.FromBase64String(data);
            var image = new BitmapImage();
            using (var stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(byteArray.AsBuffer());
                stream.Seek(0);
                await image.SetSourceAsync(stream);
            }

            return image;
        }

        public static BitmapImage ImageFromAssetsFile(string fileName)
        {
            var image = new BitmapImage(new Uri($"ms-appx:///Assets/{fileName}"));
            return image;
        }
//}]}
    }
}
