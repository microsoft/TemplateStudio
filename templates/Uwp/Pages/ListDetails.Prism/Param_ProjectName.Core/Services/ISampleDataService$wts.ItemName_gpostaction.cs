//{**
// This code block adds the method `GetListDetailDataAsync()` to the SampleDataService of your project.
//**}
    public interface ISampleDataService
    {
//^^
//{[{

        Task<IEnumerable<SampleOrder>> GetListDetailDataAsync();
//}]}
    }
}