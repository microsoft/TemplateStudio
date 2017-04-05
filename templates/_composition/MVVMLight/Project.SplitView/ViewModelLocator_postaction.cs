public class ViewModelLocator
{
    public ViewModelLocator()
    {
        //^^
        RegisterShell();
    }

    public ShellViewModel ShellViewModel => ServiceLocator.Current.GetInstance<ShellViewModel>();
    public void RegisterShell() => SimpleIoc.Default.Register<ShellViewModel>();
}
