namespace Param_RootNamespace.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
//^^
//{[{
            SimpleIoc.Default.Register<ShellViewModel>();
//}]}
        }

//{[{
        public ShellViewModel ShellViewModel => ServiceLocator.Current.GetInstance<ShellViewModel>();
//}]}
    }
}
