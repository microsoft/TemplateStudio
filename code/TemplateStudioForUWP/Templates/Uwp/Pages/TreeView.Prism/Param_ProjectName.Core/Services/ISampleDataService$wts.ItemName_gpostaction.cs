//{**
// This code block adds the method `GetTreeViewDataAsync()` to the SampleDataService of your project.
//**}
    public interface ISampleDataService
    {
//^^
//{[{

        Task<IEnumerable<SampleCompany>> GetTreeViewDataAsync();
//}]}
    }
}