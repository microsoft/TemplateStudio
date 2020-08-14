using System.Threading.Tasks;

namespace WinUIDesktopApp.Activation
{
    public interface IActivationHandler
    {
        bool CanHandle(object args);

        Task HandleAsync(object args);
    }
}
