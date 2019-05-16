//{**
// This code block adds the method `GetChartSampleDataAsync()` to the SampleDataService of your project.
//**}
    public interface ISampleDataService
    {
//^^
//{[{

        Task<ObservableCollection<DataPoint>> GetChartSampleDataAsync();
//}]}
    }
}