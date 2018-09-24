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
        public ShellViewModel ShellViewModel => ServiceLocator.Current.GetInstance<ShellViewModel>();
//}]}
    }
}
