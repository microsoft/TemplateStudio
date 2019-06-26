//{**
// This code block adds the method `GetGallerySampleDataAsync(string localResourcesPath)` to the SampleDataService of your project.
//**}
    public interface ISampleDataService
    {
//^^
//{[{

        Task<ObservableCollection<SampleImage>> GetGallerySampleDataAsync(string localResourcesPath);
//}]}
    }
}