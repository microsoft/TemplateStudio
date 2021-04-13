//{[{
using Param_RootNamespace.Activation;
using Param_RootNamespace.Contracts.Activation;
//}]}
namespace Param_RootNamespace.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            // Activation Handlers
//{[{
            SimpleIoc.Default.Register<ToastNotificationActivationHandler>();
            SimpleIoc.Default.Register<IActivationHandler>(() => SimpleIoc.Default.GetInstance<ToastNotificationActivationHandler>(), "toast");
//}]}
            // Services
//{[{
            SimpleIoc.Default.Register<IToastNotificationsService, ToastNotificationsService>();
//}]}
        }
    }
}