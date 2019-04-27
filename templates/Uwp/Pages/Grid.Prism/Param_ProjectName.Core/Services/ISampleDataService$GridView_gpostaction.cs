//{**
// This code block adds the method `GetGridSampleDataAsync()` to the SampleDataService of your project.
//**}
    public interface ISampleDataService
    {
//^^
//{[{

        ObservableCollection<SampleOrder> GetGridSampleData();
//}]}
    }
}