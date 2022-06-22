using System.IO;
using System.Windows.Media.Imaging;

namespace Param_RootNamespace.Helpers;

public static class ImageHelper
{
    public static BitmapImage ImageFromString(string data)
    {
        var image = new BitmapImage();
        byte[] binaryData = Convert.FromBase64String(data);
        image.BeginInit();
        image.StreamSource = new MemoryStream(binaryData);
        image.EndInit();
        return image;
    }

    public static BitmapImage ImageFromAssetsFile(string fileName)
    {
        var imageUri = new Uri($"pack://application:,,,/Assets/{fileName}");
        var image = new BitmapImage(imageUri);
        return image;
    }
}
