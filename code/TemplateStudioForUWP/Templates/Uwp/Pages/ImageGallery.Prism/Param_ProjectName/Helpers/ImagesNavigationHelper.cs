using System.Collections.Generic;
using System.Linq;

namespace Param_RootNamespace.Helpers
{
    public static class ImagesNavigationHelper
    {
        private static Dictionary<string, Stack<string>> _imageGalleriesHistories = new Dictionary<string, Stack<string>>();

        public static void AddImageId(string imageGalleryId, string imageId)
        {
            var stack = GetStack(imageGalleryId);
            stack.Push(imageId);
        }

        public static void UpdateImageId(string imageGalleryId, string imageId)
        {
            var stack = GetStack(imageGalleryId);
            if (stack.Any())
            {
                stack.Pop();
            }

            stack.Push(imageId);
        }

        public static string GetImageId(string imageGalleryId)
        {
            var stack = GetStack(imageGalleryId);
            return stack.Any() ? stack.Peek() : string.Empty;
        }

        public static void RemoveImageId(string imageGalleryId)
        {
            var stack = GetStack(imageGalleryId);
            if (stack.Any())
            {
                stack.Pop();
            }
        }

        private static Stack<string> GetStack(string imageGalleryId)
        {
            if (!_imageGalleriesHistories.Keys.Contains(imageGalleryId))
            {
                _imageGalleriesHistories.Add(imageGalleryId, new Stack<string>());
            }

            return _imageGalleriesHistories[imageGalleryId];
        }
    }
}
