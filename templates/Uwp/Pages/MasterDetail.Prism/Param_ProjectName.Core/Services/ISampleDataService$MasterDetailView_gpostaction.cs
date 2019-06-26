//{**
// This code block adds the method `GetMasterDetailDataAsync()` to the SampleDataService of your project.
//**}
    public interface ISampleDataService
    {
//^^
//{[{

        Task<IEnumerable<SampleOrder>> GetMasterDetailDataAsync();
//}]}
    }
}