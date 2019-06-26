//{**
// This code block adds the method `GetGallerySampleDataAsync(string localResourcesPath)` to the SampleDataService of your project.
//**}
namespace Param_RootNamespace.Core.Services
{
    public static class SampleDataService
    {
//{[{
        private static ObservableCollection<SampleImage> _gallerySampleData;
//}]}

        private static async Task<IEnumerable<SampleOrder>> GetAllOrdersAsync()
        {
        }
//^^
//{[{

        // TODO WTS: Remove this once your image gallery page is displaying real data.
        public static async Task<ObservableCollection<SampleImage>> GetGallerySampleDataAsync(string localResourcesPath)
        {
            await Task.CompletedTask;
            if (_gallerySampleData == null)
            {
                _gallerySampleData = new ObservableCollection<SampleImage>();
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

            return _gallerySampleData;
        }
//}]}
    }
}