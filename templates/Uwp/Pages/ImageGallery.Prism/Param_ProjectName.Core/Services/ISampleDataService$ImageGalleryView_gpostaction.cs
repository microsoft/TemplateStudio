//{**
// This code block adds the method `GetGallerySampleDataAsync()` to the SampleDataService of your project.
//**}
    public interface ISampleDataService
    {
//^^
//{[{

        void Initialize(string localResourcesPath);

        Task<ObservableCollection<SampleImage>> GetGallerySampleDataAsync();
//}]}
    }
}