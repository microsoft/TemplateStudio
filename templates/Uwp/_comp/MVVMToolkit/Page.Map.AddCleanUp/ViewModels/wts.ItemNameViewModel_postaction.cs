namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameViewModel : ObservableObject
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
