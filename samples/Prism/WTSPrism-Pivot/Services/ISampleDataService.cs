using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WTSPrism.Models;

namespace WTSPrism.Services
{
    public interface ISampleDataService
    {
        ObservableCollection<DataPoint> GetChartSampleData();
        ObservableCollection<Order> GetGridSampleData();
        Task<IEnumerable<Order>> GetSampleModelDataAsync();
    }
}
