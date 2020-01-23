public class ViewModelLocator
{
//^^
//{[{
    public LogInViewModel LogInViewModel
        => SimpleIoc.Default.GetInstance<LogInViewModel>();
//}]}

    public ViewModelLocator()
    {
        // Core Services
//^^
//{[{
        SimpleIoc.Default.Register<IMicrosoftGraphService, MicrosoftGraphService>();
        SimpleIoc.Default.Register<IIdentityService, IdentityService>();
//}]}
        // Services
//^^
//{[{
        SimpleIoc.Default.Register<IUserDataService, UserDataService>();
//}]}

        // Window
//^^
//{[{
        SimpleIoc.Default.Register<ILogInWindow, LogInWindow>();
        SimpleIoc.Default.Register<LogInViewModel>();
//}]}

        // Pages
    }
}
