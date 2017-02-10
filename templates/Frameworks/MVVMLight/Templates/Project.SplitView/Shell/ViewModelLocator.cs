using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;

using RootNamespace.Shell;

namespace RootNamespace
{
    public partial class ViewModelLocator
    {
        public ShellViewModel ShellViewModel => ServiceLocator.Current.GetInstance<ShellViewModel>();

        public void RegisterShell() => SimpleIoc.Default.Register<ShellViewModel>();
    }
}