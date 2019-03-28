using System;
using Caliburn.Micro;
using WinUI = Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls;

namespace Param_RootNamespace.Views
{
    public interface IShellView
    {
        INavigationService CreateNavigationService(WinRTContainer container);

        WinUI.NavigationView GetNavigationView();

        Frame GetFrame();
    }
}
