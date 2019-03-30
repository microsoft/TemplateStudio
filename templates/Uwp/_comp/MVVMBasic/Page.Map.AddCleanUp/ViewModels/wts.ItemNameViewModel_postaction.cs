namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameViewModel : Observable
    {
        //^^

        //{[{
        public void Cleanup()
        {
            if (_locationService != null)
            {
                _locationService.PositionChanged -= LocationService_PositionChanged;
                _locationService.StopListening();
            }
        }
        //}]}
    }
}
