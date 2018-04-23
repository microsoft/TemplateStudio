using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace Param_ItemNamespace.Services
{
    internal interface IBackgroundTaskService
    {
        Task RegisterBackgroundTasksAsync();

        void Start(IBackgroundTaskInstance taskInstance);
    }
}
