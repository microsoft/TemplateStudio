using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace Param_RootNamespace.Services
{
    internal interface IBackgroundTaskService
    {
        Task RegisterBackgroundTasksAsync();

        void Start(IBackgroundTaskInstance taskInstance);
    }
}
