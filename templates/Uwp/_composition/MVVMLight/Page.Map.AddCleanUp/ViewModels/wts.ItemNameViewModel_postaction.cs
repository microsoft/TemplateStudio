namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameViewModel : ViewModelBase
    {
        //^^

        //{[{
        public override void Cleanup()
        {
            if (_locationService != null)
            {
                _locationService.PositionChanged -= LocationService_PositionChanged;
                _locationService.StopListening();
            }

            base.Cleanup();
        }
        //}]}
    }
}
