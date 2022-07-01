using Microsoft.Windows.AppNotifications;

namespace Param_RootNamespace.Contracts.Services;

public interface INotificationService
{
    void Initialize();

    bool Show(string payload);

    string? ParseArguments(string args, string parameter);

    void Unregister();
}
