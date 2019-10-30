//{[{
using System.Collections.Generic;
using Prism.Windows.Navigation;
//}]}
namespace Param_RootNamespace.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
//^^
//{[{

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            await InitializeAsync();
        }
//}]}
    }
}
