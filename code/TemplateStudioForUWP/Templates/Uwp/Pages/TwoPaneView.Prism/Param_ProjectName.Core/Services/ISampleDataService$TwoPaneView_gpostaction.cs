//{**
// This code block adds the method `GetTwoPaneViewDataAsync()` to the SampleDataService of your project.
//**}
    public interface ISampleDataService
    {
//^^
//{[{

        Task<IEnumerable<SampleOrder>> GetTwoPaneViewDataAsync();
//}]}
    }
}