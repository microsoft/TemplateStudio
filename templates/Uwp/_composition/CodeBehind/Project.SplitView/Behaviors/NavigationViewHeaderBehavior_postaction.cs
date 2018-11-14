//{[{
using wts.ItemName.Services;
//}]}

namespace wts.ItemName.Behaviors
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
    }
}
