using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using WTSGeneratedPivot.Models;

namespace WTSGeneratedPivot.Services
{
    public interface ISampleDataService
    {
        ObservableCollection<DataPoint> GetChartSampleData();

        ObservableCollection<SampleOrder> GetGridSampleData();

        ObservableCollection<SampleImage> GetGallerySampleData();

        Task<IEnumerable<SampleOrder>> GetSampleModelDataAsync();
    }
}
