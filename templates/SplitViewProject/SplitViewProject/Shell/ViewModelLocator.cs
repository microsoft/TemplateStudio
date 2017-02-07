using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using SplitViewProject.Shell;

namespace SplitViewProject
{
    public partial class ViewModelLocator
    {
        public ShellViewModel ShellViewModel => ServiceLocator.Current.GetInstance<ShellViewModel>();

        public void RegisterShell() => SimpleIoc.Default.Register<ShellViewModel>();
    }
}
