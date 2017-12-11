using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WTSPrismNavigationBase.Models;

namespace WTSPrismNavigationBase.Services
{
    public interface ISampleDataService
    {
        ObservableCollection<DataPoint> GetChartSampleData();
        ObservableCollection<Order> GetGridSampleData();
        Task<IEnumerable<Order>> GetSampleModelDataAsync();
    }
}
