using System.Collections.Generic;
using System.Threading.Tasks;

using DotNetCoreWpfApp.Core.Models;

namespace DotNetCoreWpfApp.Core.Contracts.Services
{
    public interface ISampleDataService
    {
        Task<IEnumerable<SampleOrder>> GetContentGridDataAsync();

        Task<IEnumerable<SampleOrder>> GetGridDataAsync();

        Task<IEnumerable<SampleOrder>> GetMasterDetailDataAsync();
    }
}
