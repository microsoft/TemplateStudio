using System.Threading.Tasks;

namespace WinUIMenuBarApp.Contracts.Services
{
    public interface IWindowManagerService
    {
        Task OpenInDialogAsync(string pageKey, object parameter = null);
    }
}
