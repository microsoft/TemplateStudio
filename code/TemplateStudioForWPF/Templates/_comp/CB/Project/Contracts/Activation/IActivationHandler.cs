using System.Threading.Tasks;

namespace Param_RootNamespace.Contracts.Activation;

public interface IActivationHandler
{
    bool CanHandle();

    Task HandleAsync();
}
