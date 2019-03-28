namespace Param_RootNamespace.Behaviors
{
    public class NavigationViewHeaderBehavior : Behavior<WinUI.NavigationView>
    {
        //^^
        //{[{
        public void Initialize(Frame frame)
        {
            frame.Navigated += OnNavigated;
        }

        //}]}
        protected override void OnAttached()
        {
        }
    }
}