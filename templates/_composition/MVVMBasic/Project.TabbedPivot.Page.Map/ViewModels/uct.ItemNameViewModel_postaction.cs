namespace ItemNamespace.ViewModels
{
    public class uct.ItemNameViewModel : Observable
    {
        //^^

        //{[{
        public void Cleanup()
        {
            if (locationService != null)
            {
                locationService.PositionChanged -= LocationServicePositionChanged;
                locationService.StopListening();
            }
        }
        //}]}
    }
}
