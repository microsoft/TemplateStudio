public class ViewModelLocator
{
    public ViewModelLocator()
    {
        // Core Services
//{[{
        SimpleIoc.Default.Register<IMicrosoftGraphService, MicrosoftGraphService>();
        SimpleIoc.Default.Register<IIdentityService, IdentityService>();
//}]}
        // Services
//{[{
        SimpleIoc.Default.Register<IUserDataService, UserDataService>();
//}]}
    }
}
