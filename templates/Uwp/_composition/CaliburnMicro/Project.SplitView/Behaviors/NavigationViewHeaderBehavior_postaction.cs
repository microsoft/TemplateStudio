//{[{
using Caliburn.Micro;
//}]}

namespace wts.ItemName.Behaviors
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