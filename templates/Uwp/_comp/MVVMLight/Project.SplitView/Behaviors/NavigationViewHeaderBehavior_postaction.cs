//{[{
using Param_RootNamespace.ViewModels;
//}]}

namespace Param_RootNamespace.Behaviors
{
    public class NavigationViewHeaderBehavior : Behavior<WinUI.NavigationView>
    {
        protected override void OnAttached()
        {
            //^^
            //{[{
            ViewModelLocator.Current.NavigationService.Navigated += OnNavigated;
            //}]}
        }

        protected override void OnDetaching()
        {
            //^^
            //{[{
            ViewModelLocator.Current.NavigationService.Navigated -= OnNavigated;
            //}]}
        }
    }
}