namespace Param_RootNamespace.ViewModels
{
    public class ViewModelLocator
    {
        private ViewModelLocator()
        {
            //^^
            //{[{
            Register<LogInViewModel, LogInPage>();
            //}]}
        }

        //{[{
        public LogInViewModel LogInViewModel => SimpleIoc.Default.GetInstance<LogInViewModel>();
        //}]}
    }
}
