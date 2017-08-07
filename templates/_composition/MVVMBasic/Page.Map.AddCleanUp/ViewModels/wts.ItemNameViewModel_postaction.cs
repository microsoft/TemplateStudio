namespace Param_ItemNamespace.ViewModels
{
    public class wts.ItemNameViewModel : Observable
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
