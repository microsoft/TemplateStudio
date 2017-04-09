namespace ItemNamespace.ViewModels
{
    public class uct.ItemNameViewModel : ViewModelBase
    {
        //^^

        //{[{
        public override void Cleanup()
        {
            if (locationService != null)
            {
                locationService.PositionChanged -= LocationServicePositionChanged;
                locationService.StopListening();
            }

            base.Cleanup();
        }
        //}]}
    }
}
