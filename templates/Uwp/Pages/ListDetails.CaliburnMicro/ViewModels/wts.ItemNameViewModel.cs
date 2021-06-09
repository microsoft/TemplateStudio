using System;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Param_RootNamespace.Core.Services;

namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameViewModel : Conductor<wts.ItemNameDetailViewModel>.Collection.OneActive
    {
        protected override async void OnInitialize()
        {
            base.OnInitialize();

            await LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            Items.Clear();

            var data = await SampleDataService.GetListDetailsDataAsync();

            Items.AddRange(data.Select(d => new wts.ItemNameDetailViewModel(d)));
        }
    }
}
