//{**
// This code block adds the method `GetImageGalleryDataAsync(string localResourcesPath)` to the SampleDataService of your project.
//**}
    public interface ISampleDataService
    {
//^^
//{[{

        Task<IEnumerable<SampleImage>> GetImageGalleryDataAsync(string localResourcesPath);
//}]}
    }
}