//{**
// This code block adds the method `GetImageGalleryDataAsync(string localResourcesPath)` to the SampleDataService of your project.
//**}
    public interface ISampleDataService
    {
//^^
//{[{

        Task<ObservableCollection<SampleImage>> GetImageGalleryDataAsync(string localResourcesPath);
//}]}
    }
}