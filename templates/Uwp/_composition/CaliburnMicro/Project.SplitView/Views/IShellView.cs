using System;
using Caliburn.Micro;
using Microsoft.UI.Xaml.Controls;

namespace wts.ItemName.Views
{
    public interface IShellView
    {
        INavigationService CreateNavigationService(WinRTContainer container);

        NavigationView GetNavigationView();
    }
}
