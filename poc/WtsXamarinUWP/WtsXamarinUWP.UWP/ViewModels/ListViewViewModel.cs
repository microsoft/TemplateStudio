using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Toolkit.Uwp.UI.Controls;

using WtsXamarinUWP.Core.Helpers;
using WtsXamarinUWP.Core.Services;
using WtsXamarinUWP.UWP.Models;
using WtsXamarinUWP.UWP.Services;

namespace WtsXamarinUWP.UWP.ViewModels
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
