using System;
using Microsoft.UI.Xaml.Controls;

namespace WinUIDesktopApp.Contracts.Services
{
    public interface IPageService
    {
        Type GetPageType(string key);
    }
}
