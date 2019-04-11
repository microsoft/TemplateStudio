//{[{
using Param_RootNamespace.ViewModels;
//}]}
namespace Param_RootNamespace.Activation
{
    internal class DefaultActivationHandler : ActivationHandler<IActivatedEventArgs>
    {
//{[{
        private readonly string _navElement;

        public NavigationServiceEx NavigationService => ViewModelLocator.Current.NavigationService;

        public DefaultActivationHandler(Type navElement)
        {
            _navElement = navElement.FullName;
        }

//}]}
    }
}
