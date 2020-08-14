using System.Threading.Tasks;

namespace WinUIDesktopApp.Contracts.Services
{
    public interface IActivationService
    {
        Task ActivateAsync(object activationArgs);
    }
}
