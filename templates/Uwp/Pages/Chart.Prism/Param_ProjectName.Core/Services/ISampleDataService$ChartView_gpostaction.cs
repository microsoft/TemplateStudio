//{**
// This code block adds the method `GetChartDataAsync()` to the SampleDataService of your project.
//**}
    public interface ISampleDataService
    {
//^^
//{[{

        Task<ObservableCollection<DataPoint>> GetChartDataAsync();
//}]}
    }
}