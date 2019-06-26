//{**
// This code block adds the method `GetImageGalleryDataAsync(string localResourcesPath)` to the SampleDataService of your project.
//**}
//{[{
using System.Threading.Tasks;
//}]}
namespace Param_RootNamespace.Core.Services
{
    public class SampleDataService : ISampleDataService
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
        public async Task<ObservableCollection<SampleImage>> GetImageGalleryDataAsync(string localResourcesPath)
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