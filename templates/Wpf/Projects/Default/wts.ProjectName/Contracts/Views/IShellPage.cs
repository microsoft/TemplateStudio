using System.Windows.Controls;

namespace Param_RootNamespace.Contracts.Views
{
    public interface IShellPage
    {
        Frame GetNavigationFrame();

        void ShowWindow();
    }
}
