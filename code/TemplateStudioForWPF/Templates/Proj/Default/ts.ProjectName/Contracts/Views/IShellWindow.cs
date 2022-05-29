﻿using System.Windows.Controls;

namespace Param_RootNamespace.Contracts.Views
{
    public interface IShellWindow
    {
        Frame GetNavigationFrame();

        void ShowWindow();

        void CloseWindow();
    }
}
