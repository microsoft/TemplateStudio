public ViewModelLocator()
{
    // Core Services
//{[{
    SimpleIoc.Default.Register<ISampleDataService, SampleDataService>();
//}]}
}