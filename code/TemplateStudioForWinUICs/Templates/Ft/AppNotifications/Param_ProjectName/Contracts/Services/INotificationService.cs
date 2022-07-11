namespace Param_RootNamespace.Contracts.Services;

public interface INotificationService
{
    void Initialize();

    bool Show(string payload);

    string? ParseArguments(string arguments, string parameter);

    void Unregister();
}
