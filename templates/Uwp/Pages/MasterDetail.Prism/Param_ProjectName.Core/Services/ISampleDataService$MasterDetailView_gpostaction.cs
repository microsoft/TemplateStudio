//{**
// This code block adds the method `GetChartSampleData()` to the SampleDataService of your project.
//**}
    public interface ISampleDataService
    {
//^^
//{[{

        Task<IEnumerable<SampleOrder>> GetSampleModelDataAsync();
//}]}
    }
}