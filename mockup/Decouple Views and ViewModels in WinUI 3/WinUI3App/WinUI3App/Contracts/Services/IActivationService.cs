using System.Threading.Tasks;

namespace WinUI3App.Contracts.Services
{
    public interface IActivationService
    {
        Task ActivateAsync(object activationArgs);
    }
}
