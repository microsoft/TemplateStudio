using DotNetCoreWpfApp.Models;

namespace DotNetCoreWpfApp.Contracts.Services
{
    public interface IThemeSelectorService
    {
        bool SetTheme(AppTheme? theme = null);

        AppTheme GetCurrentTheme();
    }
}
