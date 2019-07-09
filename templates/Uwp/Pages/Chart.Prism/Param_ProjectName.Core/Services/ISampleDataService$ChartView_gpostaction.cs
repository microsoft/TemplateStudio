//{**
// This code block adds the method `GetChartDataAsync()` to the SampleDataService of your project.
//**}
    public interface ISampleDataService
    {
//^^
//{[{

        Task<IEnumerable<DataPoint>> GetChartDataAsync();
//}]}
    }
}