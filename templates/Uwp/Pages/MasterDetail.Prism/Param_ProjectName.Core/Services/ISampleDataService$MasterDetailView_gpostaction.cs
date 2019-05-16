//{**
// This code block adds the method `GetSampleModelDataAsync()` to the SampleDataService of your project.
//**}
    public interface ISampleDataService
    {
//^^
//{[{

        Task<IEnumerable<SampleOrder>> GetSampleModelDataAsync();
//}]}
    }
}