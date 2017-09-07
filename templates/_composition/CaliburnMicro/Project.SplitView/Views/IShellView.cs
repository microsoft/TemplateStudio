using System;
using Caliburn.Micro;

namespace wts.ItemName.Views
{
    public interface IShellView
    {
        INavigationService CreateNavigationService(WinRTContainer container);
    }
}
