public ViewModelLocator()
{
    // Core Services
//{[{
    SimpleIoc.Default.Register<IApplicationInfoService, ApplicationInfoService>();
//}]}
}