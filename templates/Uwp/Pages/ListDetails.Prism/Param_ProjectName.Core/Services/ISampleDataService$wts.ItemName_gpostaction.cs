//{**
// This code block adds the method `GetListDetailsDataAsync()` to the SampleDataService of your project.
//**}
    public interface ISampleDataService
    {
//^^
//{[{

        Task<IEnumerable<SampleOrder>> GetListDetailsDataAsync();
//}]}
    }
}