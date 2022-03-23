namespace Param_RootNamespace.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
//^^
//{[{

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatingFrom(e, viewModelState, suspending);
            UnregisterEvents();
        }
//}]}
    }
}
