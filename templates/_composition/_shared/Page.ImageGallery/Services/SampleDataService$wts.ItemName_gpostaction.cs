//{**
// This code block adds the method `GetSampleModelDataAsync()` to the SampleDataService of your project.
//**}
//{[{
using Param_ItemNamespace.Helpers;
//}]}
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
            var name = "SampleImage_Name".GetLocalized();
            for (int i = 1; i <= 10; i++)
            {
                data.Add(new SampleImage()
                {
                    Source = $"ms-appx:///Assets/SampleData/SamplePhoto{i}.png",
                    Name = $"{name} {i}"
                });
            }

            return data;
        }
//}]}
    }
}