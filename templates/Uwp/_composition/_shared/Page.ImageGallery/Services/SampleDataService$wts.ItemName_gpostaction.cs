//{**
// This code block adds the method `GetSampleModelDataAsync()` to the SampleDataService of your project.
//**}
namespace Param_ItemNamespace.Services
{
    public static class SampleDataService
    {

//^^
//{[{
        // TODO WTS: Remove this once your image gallery page is displaying real data
        public static ObservableCollection<SampleImage> GetGallerySampleData()
        {
            var data = new ObservableCollection<SampleImage>();
            for (int i = 1; i <= 10; i++)
            {
                data.Add(new SampleImage()
                {
                    ID = $"{i}",
                    Source = $"ms-appx:///Assets/SampleData/SamplePhoto{i}.png",
                    Name = $"Image sample {i}"
                });
            }

            return data;
        }
//}]}
    }
}