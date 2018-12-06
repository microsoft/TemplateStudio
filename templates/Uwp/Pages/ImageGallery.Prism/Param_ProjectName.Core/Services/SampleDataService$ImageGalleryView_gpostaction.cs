//{**
// This code block adds the method `GetSampleModelDataAsync()` to the SampleDataService of your project.
//**}
namespace Param_ItemNamespace.Core.Services
{
    public class SampleDataService : ISampleDataService
    {
//{[{
        private static ObservableCollection<SampleImage> _gallerySampleData;
//}]}
//^^
//{[{

        // TODO WTS: Remove this once your image gallery page is displaying real data
        public ObservableCollection<SampleImage> GetGallerySampleData()
        {
            if (_gallerySampleData == null)
            {
                _gallerySampleData = new ObservableCollection<SampleImage>();
                for (int i = 1; i <= 10; i++)
                {
                    _gallerySampleData.Add(new SampleImage()
                    {
                        ID = $"{i}",
                        Source = $"ms-appx:///Assets/SampleData/SamplePhoto{i}.png",
                        Name = $"Image sample {i}"
                    });
                }
            }

            return _gallerySampleData;
        }
//}]}
    }
}