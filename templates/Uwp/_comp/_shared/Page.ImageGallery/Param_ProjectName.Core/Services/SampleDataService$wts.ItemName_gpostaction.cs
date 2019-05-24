//{**
// This code block adds the method `GetSampleModelDataAsync()` to the SampleDataService of your project.
//**}
//{[{
using System.Threading.Tasks;
//}]}
namespace Param_RootNamespace.Core.Services
{
    public static class SampleDataService
    {
//^^
//{[{

        private static string _localResourcesPath;

        private static ObservableCollection<SampleImage> _gallerySampleData;

        public static void Initialize(string localResourcesPath)
        {
            _localResourcesPath = localResourcesPath;
        }

        // TODO WTS: Remove this once your image gallery page is displaying real data.
        public static async Task<ObservableCollection<SampleImage>> GetGallerySampleDataAsync()
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
                        Source = $"{_localResourcesPath}/SampleData/SamplePhoto{i}.png",
                        Name = $"Image sample {i}"
                    });
                }
            }

            return _gallerySampleData;
        }
//}]}
    }
}