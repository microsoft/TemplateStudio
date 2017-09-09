using System;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Param_ItemNamespace.Services;

namespace Param_ItemNamespace.ViewModels
{
    public class MasterDetailsItemViewModel : Conductor<MasterDetailsItemDetailViewModel>.Collection.OneActive
    {
        protected override async void OnInitialize()
        {
            base.OnInitialize();

            await LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            Items.Clear();

            var data = await SampleDataService.GetSampleModelDataAsync();

            Items.AddRange(data.Select(d => new MasterDetailsItemDetailViewModel(d)));

            ActiveItem = Items.First();
        }
    }
}
