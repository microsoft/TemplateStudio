using App_Name.Shell;
using GalaSoft.MvvmLight.Ioc;

using Microsoft.Practices.ServiceLocation;

namespace App_Name
{
    public partial class ViewModelLocator
    {
        public ShellViewModel ShellViewModel => ServiceLocator.Current.GetInstance<ShellViewModel>();

        public void RegisterShell() => SimpleIoc.Default.Register<ShellViewModel>();
    }
}
