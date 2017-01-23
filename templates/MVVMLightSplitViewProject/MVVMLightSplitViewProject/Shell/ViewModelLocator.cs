using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using MVVMLightSplitViewProject.Shell;

namespace MVVMLightSplitViewProject
{
    public partial class ViewModelLocator
    {
        public ShellViewModel ShellViewModel => ServiceLocator.Current.GetInstance<ShellViewModel>();

        public void RegisterShell() => SimpleIoc.Default.Register<ShellViewModel>();
    }
}

