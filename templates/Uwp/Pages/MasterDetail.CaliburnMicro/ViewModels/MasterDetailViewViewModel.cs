using System;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Param_RootNamespace.Core.Services;

namespace Param_RootNamespace.ViewModels
{
    public class MasterDetailViewViewModel : Conductor<MasterDetailViewDetailViewModel>.Collection.OneActive
    {
        protected override async void OnInitialize()
        {
            base.OnInitialize();

            await LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            Items.Clear();

            var data = await SampleDataService.GetMasterDetailDataAsync();

            Items.AddRange(data.Select(d => new MasterDetailViewDetailViewModel(d)));
        }
    }
}
