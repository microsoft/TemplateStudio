//{**
// This code block adds the method `GetImageGalleryDataAsync(string localResourcesPath)` to the SampleDataService of your project.
//**}
namespace Param_RootNamespace.Core.Services
{
    public static class SampleDataService
    {
//{[{
        private static ICollection<SampleImage> _gallerySampleData;
//}]}

        private static IEnumerable<SampleOrder> AllOrders()
        {
        }
//^^
//{[{

        // TODO WTS: Remove this once your image gallery page is displaying real data.
        public static async Task<IEnumerable<SampleImage>> GetImageGalleryDataAsync(string localResourcesPath)
        {
            if (_gallerySampleData == null)
            {
                _gallerySampleData = new List<SampleImage>();
                for (int i = 1; i <= 10; i++)
                {
                    _gallerySampleData.Add(new SampleImage()
                    {
                        ID = $"{i}",
                        Source = $"{localResourcesPath}/SampleData/SamplePhoto{i}.png",
                        Name = $"Image sample {i}"
                    });
                }
            }

            await Task.CompletedTask;
            return _gallerySampleData;
        }
//}]}
    }
}