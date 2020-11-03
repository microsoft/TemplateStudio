using System.Collections.Generic;
using System.Threading.Tasks;

using WinUIDesktopApp.Core.Models;

namespace WinUIDesktopApp.Core.Contracts.Services
{
    public interface ISampleDataService
    {
        Task<IEnumerable<SampleOrder>> GetContentGridDataAsync();

        Task<IEnumerable<SampleOrder>> GetGridDataAsync();

        Task SaveOrderAsync(SampleOrder order);

        Task<IEnumerable<SampleOrder>> GetMasterDetailDataAsync();
    }
}
