using System.Windows.Controls;

namespace DotNetCoreWpfApp.Contracts.Views
{
    public interface IShellWindow
    {
        Frame GetNavigationFrame();

        void ShowWindow();

        void CloseWindow();
    }
}
