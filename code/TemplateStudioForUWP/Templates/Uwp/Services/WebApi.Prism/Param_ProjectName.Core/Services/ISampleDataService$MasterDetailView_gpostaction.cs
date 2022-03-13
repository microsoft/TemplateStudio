//{**
// This code block adds the method `GetWebApiSampleDataAsync()` to the SampleDataService of your project.
//**}
    public interface ISampleDataService
    {
//^^
//{[{

        Task<IEnumerable<SampleCompany>> GetWebApiSampleDataAsync();
//}]}
    }
}