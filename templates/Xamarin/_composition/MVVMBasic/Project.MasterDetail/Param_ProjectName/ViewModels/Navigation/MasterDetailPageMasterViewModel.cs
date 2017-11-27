using System.Collections.ObjectModel;
using Param_RootNamespace.Helpers;
using Param_RootNamespace.Models;
using Param_RootNamespace.Views;

namespace Param_RootNamespace.ViewModels.Navigation
{
    public class MasterDetailPageMasterViewModel : Observable
    {
        public MasterDetailPageMasterViewModel()
        {
        }

        public ObservableCollection<MasterDetailPageMenuItem> PrimaryMenuItems { get; private set; } = new ObservableCollection<MasterDetailPageMenuItem>
        {
        };
    }
}
