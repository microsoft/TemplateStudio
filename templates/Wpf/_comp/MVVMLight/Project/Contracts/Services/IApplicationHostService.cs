using System.Threading.Tasks;

namespace Param_RootNamespace.Contracts.Services
{
    public interface IApplicationHostService
    {
        Task StartAsync();

        Task StopAsync();
    }
}
