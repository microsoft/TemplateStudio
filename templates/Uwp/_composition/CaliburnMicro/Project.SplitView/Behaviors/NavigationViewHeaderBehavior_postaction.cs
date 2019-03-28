//{[{
using Caliburn.Micro;
//}]}

namespace Param_RootNamespace.Behaviors
{
    public class NavigationViewHeaderBehavior : Behavior<WinUI.NavigationView>
    {
        //^^
        //{[{
        public void Initialize(INavigationService navigationService)
        {
            navigationService.Navigated += OnNavigated;
        }

        //}]}
        protected override void OnAttached()
        {
        }
    }
}