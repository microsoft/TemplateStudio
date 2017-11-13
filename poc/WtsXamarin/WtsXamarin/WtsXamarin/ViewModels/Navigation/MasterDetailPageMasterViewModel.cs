using System.Collections.ObjectModel;
using WtsXamarin.Helpers;
using WtsXamarin.Services;
using WtsXamarin.Views.Navigation;

namespace WtsXamarin.ViewModels.Navigation
{
    public class MasterDetailPageMasterViewModel : Observable
    {
        public ObservableCollection<MasterDetailPageMenuItem> PrimaryMenuItems { get; private set; }

        public MasterDetailPageMasterViewModel()
        {
            var primaryItems = NavigationService.PrimaryNavigationItems;
            PrimaryMenuItems = new ObservableCollection<MasterDetailPageMenuItem>(primaryItems);
        }
    }
}