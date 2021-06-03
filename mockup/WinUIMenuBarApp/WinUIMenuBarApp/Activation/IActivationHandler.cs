using System.Threading.Tasks;

namespace WinUIMenuBarApp.Activation
{
    public interface IActivationHandler
    {
        bool CanHandle(object args);

        Task HandleAsync(object args);
    }
}
