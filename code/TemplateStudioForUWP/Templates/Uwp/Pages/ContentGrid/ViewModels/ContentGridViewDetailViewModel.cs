using System;
using System.Linq;
using System.Threading.Tasks;
using Param_RootNamespace.Core.Models;
using Param_RootNamespace.Core.Services;

namespace Param_RootNamespace.ViewModels
{
    public class ContentGridViewDetailViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private SampleOrder _item;

        public SampleOrder Item
        {
            get { return _item; }
            set { Param_Setter(ref _item, value); }
        }

        public ContentGridViewDetailViewModel()
        {
        }

        public async Task InitializeAsync(long orderID)
        {
            var data = await SampleDataService.GetContentGridDataAsync();
            Item = data.First(i => i.OrderID == orderID);
        }
    }
}
