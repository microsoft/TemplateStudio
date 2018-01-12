using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Toolkit.Uwp.UI.Controls;

using Wts.Core.Helpers;
using Wts.Core.Services;
using Wts.UWP.Models;
using Wts.UWP.Services;

namespace Wts.UWP.ViewModels
{
    public class ListViewViewModel : Observable
    {
        private SampleOrderWithSymbol _selected;

        public SampleOrderWithSymbol Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ObservableCollection<SampleOrderWithSymbol> SampleItems { get; private set; } = new ObservableCollection<SampleOrderWithSymbol>();

        public ListViewViewModel()
        {
        }

        public async Task LoadDataAsync(MasterDetailsViewState viewState)
        {
            SampleItems.Clear();

            var data = await SampleDataService.GetAllOrdersAsync();

            foreach (var item in data)
            {
                SampleItems.Add(new SampleOrderWithSymbol(item));
            }

            if (viewState == MasterDetailsViewState.Both)
            {
                Selected = SampleItems.First();
            }
        }
    }
}
