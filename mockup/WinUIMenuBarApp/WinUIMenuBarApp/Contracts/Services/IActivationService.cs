using System.Threading.Tasks;

namespace WinUIMenuBarApp.Contracts.Services
{
    public interface IActivationService
    {
        Task ActivateAsync(object activationArgs);
    }
}
