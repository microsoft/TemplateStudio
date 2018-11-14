//{[{
using wts.ItemName.ViewModels;
//}]}

namespace wts.ItemName.Behaviors
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
    }
}