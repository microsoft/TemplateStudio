namespace Param_RootNamespace.ViewModels
{
    public class ViewModelLocator
    {
        private ViewModelLocator()
        {
//^^
//{[{
            SimpleIoc.Default.Register<ShellViewModel>();
//}]}
        }

//{[{
        public ShellViewModel ShellViewModel => SimpleIoc.Default.GetInstance<ShellViewModel>();
//}]}
    }
}
