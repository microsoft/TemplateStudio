//{[{
using Param_RootNamespace.Services;
//}]}

namespace Param_RootNamespace.Behaviors
{
    public class NavigationViewHeaderBehavior : Behavior<WinUI.NavigationView>
    {
        protected override void OnAttached()
        {
            //^^
            //{[{
            NavigationService.Navigated += OnNavigated;
            //}]}
        }

        protected override void OnDetaching()
        {
            //^^
            //{[{
            NavigationService.Navigated -= OnNavigated;
            //}]}
        }
    }
}
